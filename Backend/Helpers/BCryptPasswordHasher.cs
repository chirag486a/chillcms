using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

namespace Backend.Helpers
{
    public class BCryptPasswordHasher : IPasswordHasher<AppUser>
    {
        public string HashPassword(AppUser user, string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(AppUser user, string hashedPassword, string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}