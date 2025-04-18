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
        private readonly ApplicationDbContext _dbContenxt;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContenxt = dbContext;
        }
        public async Task<List<dynamic>> GetAllUsersAsync(GetAllUsersQueryDto queryDto)
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

                users = users.SortField(queryDto.SortBy, queryDto.IsDescending, "CreatedAt");


                var results = await users.SelectDynamic(queryDto.Fields).SelectDynamicExcluding(queryDto.ExcludeFields).ToListAsync();
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