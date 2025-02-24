using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos.AppUser;
using Backend.Extensions;
using Backend.Interfaces.IServices;
using Backend.Mappers;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        UserManager<AppUser> _userManager;
        ITokenService _tokenService;
        SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupDto signup)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var NewUser = signup.ToAppUserFromSignupDto();

                // Done to remove editor warning...
                if (NewUser.Email == null) return BadRequest("Email is invalid");
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
            catch (Exception err)
            {
                Console.WriteLine(err);
                return Ok("Something went wrong");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                AppUser? loginUser = null;
                if (!string.IsNullOrWhiteSpace(loginDto.Email))
                    loginUser = await _userManager.FindByEmailAsync(loginDto.Email);

                if (loginUser == null)
                {
                    var errorResponse = new
                    {
                        message = "Could not find user"
                    };
                    return NotFound(errorResponse);
                }

                var results = await _signInManager.CheckPasswordSignInAsync(loginUser, loginDto.Password, false);

                if (!results.Succeeded)
                {
                    var errorResponse = new
                    {
                        message = "Username not found or Incorrect Password"
                    };

                    return Unauthorized(errorResponse);
                }

                var response = new
                {
                    Name = loginUser.Name,
                    Email = loginUser.Email,
                    Token = _tokenService.GenerateToken(loginUser),
                };
                return Ok(response);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                var errResponse = new
                {
                    message = "Something went wrong"
                };
                return BadRequest(errResponse);
            }
        }

        [Authorize]
        [HttpPatch("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordDto passwordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var email = User.GetEmail();

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var errorResponse = new
                    {
                        message = "User not logged in."
                    };
                    return Unauthorized(errorResponse);
                }


                var result = await _userManager.ChangePasswordAsync(user, passwordDto.OldPassword, passwordDto.OldPassword);

                if (!result.Succeeded)
                {
                    return Ok(result.Errors);
                }

                var response = new
                {
                    status = "success",
                    message = "Change Passworrd successfully",
                };
                return Ok(response);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(new
                {
                    message = "Something went wrong"
                });
            }
        }

    }
}