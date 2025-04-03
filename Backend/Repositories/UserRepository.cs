using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos.User;
using Backend.Interfaces.IRepository;

namespace Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContenxt;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContenxt = dbContext;
        }
        public Task GetAllUsers(GetAllUsersQueryDto queryDto)
        {

            throw new NotImplementedException();
        }
    }
}