using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Backend.Models.Users;
using Backend.Helpers;


using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http.Features;

namespace Backend.Extensions
{
    public static class QuerableExtensions
    {
        public static IQueryable<dynamic> SelectDynamic<TEntity>(this IQueryable<TEntity> query, string fields) where TEntity : class
        {

            if (string.IsNullOrWhiteSpace(fields) || string.IsNullOrEmpty(fields))
            {
                return query;
            }

            var fieldNames = fields.Split(',')
                          .Select(f => f.Trim())
                          .Where(f => !string.IsNullOrWhiteSpace(f))
                          .ToList();

            var (isValid, invalidFields) = ValidateFields.Validate<TEntity>(fieldNames);

            if (!isValid)
            {
                throw new ArgumentException($"Invalid field names: {string.Join(", ", invalidFields)}", nameof(fields));
            }

            var entityType = typeof(TEntity);
            var parameter = Expression.Parameter(entityType, "e");

            // Build a lambda: e => new Dictionary<string, object> { ["Field"] = e.Field, ... }
            var addMethod = typeof(Dictionary<string, object>).GetMethod("Add");

            var newDict = Expression.ListInit(
                Expression.New(typeof(Dictionary<string, object>)),
                fieldNames.Select(field =>
                {
                    var prop = entityType.GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (prop == null)
                        throw new ArgumentException($"Property '{field}' does not exist");

                    var propAccess = Expression.Property(parameter, prop);
                    var boxed = Expression.Convert(propAccess, typeof(object)); // box value type

                    return Expression.ElementInit(addMethod!, Expression.Constant(prop.Name), boxed);
                })
            );

            var selector = Expression.Lambda<Func<TEntity, Dictionary<string, object>>>(newDict, parameter);

            return query.Select(selector);
        }
    //     public static IQueryable<dynamic> SelectDynamicExcluding<TEntity>(
    // this IQueryable<TEntity> query, string excludeFields) where TEntity : class
    //     {
    //         var excluded = excludeFields.Split(',')
    //                                      .Select(f => f.Trim())
    //                                      .Where(f => !string.IsNullOrWhiteSpace(f))
    //                                      .ToHashSet(StringComparer.OrdinalIgnoreCase);

    //         var entityType = typeof(TEntity);
    //         var properties = entityType
    //             .GetProperties(BindingFlags.Public | BindingFlags.Instance)
    //             .Where(p => !excluded.Contains(p.Name))
    //             .ToList();

    //         var parameter = Expression.Parameter(entityType, "e");
    //         var addMethod = typeof(Dictionary<string, object>).GetMethod("Add");

    //         var newDict = Expression.ListInit(
    //             Expression.New(typeof(Dictionary<string, object>)),
    //             properties.Select(prop =>
    //             {
    //                 var propAccess = Expression.Property(parameter, prop);
    //                 var boxed = Expression.Convert(propAccess, typeof(object));
    //                 return Expression.ElementInit(addMethod!, Expression.Constant(prop.Name), boxed);
    //             })
    //         );

    //         var selector = Expression.Lambda<Func<TEntity, Dictionary<string, object>>>(newDict, parameter);
    //         return query.Select(selector);
    //     }


        public static IQueryable<TEntity> SortField<TEntity>(this IQueryable<TEntity> query, string fields, bool IsDescending = false, string defaultField = "CreatedAt") where TEntity : class
        {
            var sortField = fields
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

            // and add orderBy or orderDescendingBy
            System.Reflection.PropertyInfo? property = null;
            var skip = 0;
            foreach (var item in sortField)
            {
                property = typeof(TEntity).GetProperty(item);
                skip++;
                if (property != null) break;
            }

            if (property == null)
            {
                property = typeof(TEntity).GetProperty(defaultField);
            }


            if (property == null)
                return query;

            var param = Expression.Parameter(typeof(TEntity), "u");
            var propertyAccess = Expression.Property(param, property);
            var conversion = Expression.Convert(propertyAccess, typeof(object));

            var orderByExp = Expression.Lambda<Func<TEntity, object>>(conversion, param);

            query = IsDescending ? query.OrderByDescending(orderByExp) : query.OrderBy(orderByExp);

            foreach (var field in sortField.Skip(skip))
            {
                property = typeof(TEntity).GetProperty(field);
                if (property == null) continue;

                propertyAccess = Expression.Property(param, property);
                conversion = Expression.Convert(propertyAccess, typeof(object));

                orderByExp = Expression.Lambda<Func<TEntity, object>>(conversion, param);

                query = IsDescending ? ((IOrderedQueryable<TEntity>)query).ThenByDescending(orderByExp) : ((IOrderedQueryable<TEntity>)query).ThenBy(orderByExp);
            }

            return query;

        }
    }
}