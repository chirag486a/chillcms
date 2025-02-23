using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Backend.Interfaces.IServices;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
    public class TokenService : ITokenService
    {
        IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(string Email)
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

            var Claims = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email, Email)
                ]
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = Claims,
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(expiresInString)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}