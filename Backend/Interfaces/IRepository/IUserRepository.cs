using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dtos.User;
using Backend.Models.Users;

namespace Backend.Interfaces.IRepository
{
    public interface IUserRepository
    {
        public Task<(List<Dictionary<string, object>> data, int total)> GetAllUsersAsync(GetAllUsersQueryDto queryDto);
    }
}