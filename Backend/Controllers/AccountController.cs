using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos.AppUser;
using Backend.Mappers;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AccountController : ControllerBase
    {
        UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupDto signup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var NewUser = signup.ToAppUserFromSignupDto();
            var ifExists = await _userManager.FindByEmailAsync(NewUser.Email);
            if (ifExists != null)
            {
                return BadRequest("User Already Exist");
            }

            var result = await _userManager.CreateAsync(NewUser, signup.Password);

            if (result.Succeeded)
            {
                return Ok(NewUser);
            }

            return Ok("Something went wrong");
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