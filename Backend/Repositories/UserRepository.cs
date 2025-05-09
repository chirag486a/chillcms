using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos.Response;
using Backend.Dtos.User;
using Backend.Extensions;
using Backend.Interfaces.IRepository;
using Backend.Models.Contents;
using Backend.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<(List<Dictionary<string, object>> data, int total)> GetAllUsersAsync(GetAllUsersQueryDto queryDto)
        {
            try
            {
                queryDto.ExcludeFields = string.Join(",", queryDto.ExcludeFields, "PasswordHash");

                IQueryable<User> users = _dbContext.Users.AsQueryable();
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

                users = users.SortField(queryDto.SortBy, queryDto.IsDescending, "CreatedAt");

                IQueryable<Dictionary<string, object>> data;
                int count = await users.CountAsync();
                users = users.Skip((queryDto.Page - 1) * queryDto.PageSize).Take(queryDto.PageSize);

                if (!string.IsNullOrEmpty(queryDto.Fields))
                {
                    data = users.SelectDynamic(queryDto.Fields);

                    return (await data.Select(u => u.ToDictionary()).ToListAsync(), count);
                }
                if (!string.IsNullOrEmpty(queryDto.ExcludeFields))
                {
                    data = users.SelectDynamicExcluding(queryDto.ExcludeFields);
                    return (await data.Select(u => u.ToDictionary()).ToListAsync(), count);
                }


                return (await users.Select(u => u.ToDictionary()).ToListAsync(), count);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }
        }
    }
}