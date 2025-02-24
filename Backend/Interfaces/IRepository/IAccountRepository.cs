using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dtos.AppUser;
using Backend.Models;

namespace Backend.Interfaces.IRepository
{
    public interface IAccountRepository
    {
        public AppUser SignUser(SignupDto signupDto);
        public AppUser LoginUser(LoginDto loginDto);
        public AppUser ChangePassword(PasswordDto loginDto);
    }
}