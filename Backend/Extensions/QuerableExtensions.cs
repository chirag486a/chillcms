using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Backend.Extensions
{
    public static class QuerableExtensions
    {
        public static IQueryable<TEntity> SelectDynamic<TEntity>(this IQueryable<TEntity> query, string fields) where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return query.Select(u => u);
            }

            // List down property
            List<string> properties = fields
                                        .Split(",")
                                        .Select(x => x.Trim())
                                        .Where(x => !string.IsNullOrWhiteSpace(x))
                                        .ToList();
            // Get Property info
            List<PropertyInfo> typeProperties = typeof(TEntity)
                                        .GetProperties()
                                        .Where(x => properties.Contains(x.Name, StringComparer.OrdinalIgnoreCase)).ToList();

            // Create parameter expression
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "e");


            // Create binding for each property
            var bindings = typeProperties.Select(p =>
                Expression.Bind(p, Expression.Property(parameter, p))
            ).ToList();

            // Create new expression using the entity type
            var newExpression = Expression.New(typeof(TEntity));

            // Create member init expression
            var memberInit = Expression.MemberInit(newExpression, bindings);

            // Create lambda expression
            var lambda = Expression.Lambda<Func<TEntity, TEntity>>(memberInit);

            return query.Select(lambda);
        }
        public static IQueryable<TEntity> SortField<TEntity>(this IQueryable<TEntity> query, string fields) where TEntity : class
        {



            throw new NotImplementedException();
        }
    }
}