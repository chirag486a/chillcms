using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos.Response;
using Backend.Dtos.User;
using Backend.Interfaces.IRepository;
using Backend.Models.Contents;
using Backend.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContenxt;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContenxt = dbContext;
        }
        public async Task<List<User>> GetAllUsersAsync(GetAllUsersQueryDto queryDto)
        {
            try
            {

                IQueryable<User> users = _dbContenxt.Users.AsQueryable();
                users = users.Skip((queryDto.Page - 1) * queryDto.PageSize).Take(queryDto.PageSize);
                if (!string.IsNullOrWhiteSpace(queryDto.Id))
                {
                    users = users.Where(u => u.Id == queryDto.Id);
                }
                if (!string.IsNullOrWhiteSpace(queryDto.Email))
                {
                    users = users.Where(u => u.Email == queryDto.Email);
                }
                if (!string.IsNullOrWhiteSpace(queryDto.UserName))
                {
                    users = users.Where(u => u.UserName == queryDto.UserName);
                }
                var sortField = queryDto.SortBy
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                // and add orderBy or orderDescendingBy
                System.Reflection.PropertyInfo? property = null;
                var skip = 0;
                foreach (var item in sortField)
                {
                    property = typeof(User).GetProperty(string.IsNullOrWhiteSpace(item) ? "CreatedAt" : item);
                    skip++;
                    if (property != null) break;
                }
                List<User> results;
                if (property == null)
                {
                    users = queryDto.IsDescending ? users.OrderByDescending(u => u.CreatedAt) : users.OrderBy(u => u.CreatedAt);

                    results = await users.ToListAsync();
                    return results;
                }

                var param = Expression.Parameter(typeof(User), "u");
                var propertyAccess = Expression.Property(param, property);
                var conversion = Expression.Convert(propertyAccess, typeof(object));

                var orderByExp = Expression.Lambda<Func<User, object>>(conversion, param);

                users = queryDto.IsDescending ? users.OrderByDescending(orderByExp) : users.OrderBy(orderByExp);

                foreach (var field in sortField.Skip(skip))
                {
                    property = typeof(User).GetProperty(field);
                    if (property == null) continue;

                    propertyAccess = Expression.Property(param, property);
                    conversion = Expression.Convert(propertyAccess, typeof(object));

                    orderByExp = Expression.Lambda<Func<User, object>>(conversion, param);

                    users = queryDto.IsDescending ? ((IOrderedQueryable<User>)users).ThenByDescending(orderByExp) : ((IOrderedQueryable<User>)users).ThenBy(orderByExp);
                }

                results = await users.ToListAsync();
                return results;
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }
        }
    }
}