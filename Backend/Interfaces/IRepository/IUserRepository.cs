using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dtos.User;

namespace Backend.Interfaces.IRepository
{
    public interface IUserRepository
    {
        public Task GetAllUsers(GetAllUsersQueryDto queryDto);
    }
}