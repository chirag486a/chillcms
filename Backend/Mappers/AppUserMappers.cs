using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dtos.AppUser;
using Backend.Models;

namespace Backend.Mappers
{
    public static class AppUserMappers
    {
        public static AppUser ToAppUserFromSignupDto(this SignupDto signupDto)
        {
            return new AppUser
            {
                UserName = signupDto.UserName,
                Name = signupDto.Name,
                Email = signupDto.Email,
            };
        }
    }
}