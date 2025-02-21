using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos.AppUser;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/")]
    public class Authentication : ControllerBase
    {
        ApplicationDbContext _context;
        public Authentication(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("signup")]
        public IActionResult Signup([FromBody] SignupDto signup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Load user manager
            // Search If user already exists
            // Create user (Repository Pattern)
            return Ok("hello");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Load user manager
            // Search If user already exists
            // Create user (Repository Pattern)
            return Ok("hello");
        }
    }
}