using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Backend.Interfaces.IServices;
using Backend.Models;
using Backend.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
    public class TokenService : ITokenService
    {
        IConfiguration _configuration;
        UserManager<User> _userManager;
        public TokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<string> GenerateTokenAsync(User user)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var keyString = jwtSettings["Key"];
            var expiresInString = jwtSettings["ExpiryInMinutes"];
            if (string.IsNullOrWhiteSpace(keyString))
            {
                throw new Exception("JWT Key is missing from configuration");
            }
            if (string.IsNullOrEmpty(expiresInString))
            {
                throw new Exception("ExpireInMinute property is missing from configuration");

            }
            var key = Encoding.ASCII.GetBytes(keyString);

            if (user.Email == null)
            {
                throw new ArgumentException("Email is null");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.GivenName, user.Name),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                ]
            );

            foreach (var role in roles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(expiresInString)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

  }
}