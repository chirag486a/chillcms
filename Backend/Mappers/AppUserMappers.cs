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
        public static AppUser ToAppUserFromSignupDto(this CreateUserDto createUserDto)
        {
            return new AppUser
            {
                UserName = createUserDto.UserName,
                Name = createUserDto.Name,
                Email = createUserDto.Email,
            };
        }
    }
}