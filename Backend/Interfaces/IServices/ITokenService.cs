using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Interfaces.IServices
{
    public interface ITokenService
    {
        public string GenerateToken(AppUser appUser);
    }
}