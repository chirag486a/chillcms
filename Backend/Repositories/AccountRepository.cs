using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dtos.AppUser;
using Backend.Interfaces.IRepository;
using Backend.Models;

namespace Backend.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public AppUser SignUser(SignupDto signupDto)
        {
            throw new NotImplementedException();
        }
        public AppUser LoginUser(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }
        public AppUser ChangePassword(PasswordDto loginDto)
        {
            throw new NotImplementedException();
        }


    }
}