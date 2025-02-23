using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Interfaces.IServices
{
    public interface ITokenService
    {
        public string GenerateToken(string Email);
    }
}