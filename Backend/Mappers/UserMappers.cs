using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dtos.User;
using Backend.Models;
using Backend.Models.Users;

namespace Backend.Mappers
{
    public static class UserMappers
    {
        public static User ToUserFromSignupDto(this CreateUserDto createUserDto)
        {
            return new User
            {
                UserName = createUserDto.UserName,
                Name = createUserDto.Name,
                Email = createUserDto.Email,
            };
        }
    }
}