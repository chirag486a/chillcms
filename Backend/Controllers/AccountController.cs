using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos.User;
using Backend.Extensions;
using Backend.Interfaces.IServices;
using Backend.Mappers;
using Backend.Models;
using Backend.Models.Users;
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
        UserManager<User> _userManager;
        ITokenService _tokenService;
        SignInManager<User> _signInManager;
        IFileService _fileService;
        public AccountController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager, IFileService fileService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _fileService = fileService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUser)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var NewUser = createUser.ToUserFromSignupDto();

                // Done to remove editor warning...
                if (NewUser.Email == null) return BadRequest("Email is invalid");
                var ifExists = await _userManager.FindByEmailAsync(NewUser.Email);
                if (ifExists != null)
                {
                    var errResponse = new
                    {
                        status = "fail",
                        errors = new[] {
                            new {
                                code = "UserAlreadyExits",
                                description = "Email with the user already exits"
                            }
                        }
                    };
                    return BadRequest(errResponse);
                }


                var result = await _userManager.CreateAsync(NewUser, createUser.Password);

                if (!result.Succeeded)
                {
                    return Ok("Something went wrong");
                }
                await _fileService.CreateUserDirectoryAsync(NewUser);
                
                return Ok(NewUser);
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
                User? loginUser = null;
                if (!string.IsNullOrWhiteSpace(loginDto.Email))
                    loginUser = await _userManager.FindByEmailAsync(loginDto.Email);

                if (loginUser == null)
                {
                    var errorResponse = new
                    {
                        status = "fail",
                        message = "Could not find user"
                    };
                    return NotFound(errorResponse);
                }

                var results = await _signInManager.CheckPasswordSignInAsync(loginUser, loginDto.Password, false);

                if (!results.Succeeded)
                {
                    var errorResponse = new
                    {
                        status = "fail",
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
                    status = "fail",
                    message = "Something went wrong"
                };
                return BadRequest(errResponse);
            }
        }

        [HttpPatch("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto passwordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var email = passwordDto.Email;

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var errorResponse = new
                    {
                        status = "fail",
                        errors = new
                        {
                            code = "UserNotFound",
                            description = "User not found"
                        }
                    };
                    return Unauthorized(errorResponse);
                }


                var result = await _userManager.ChangePasswordAsync(user, passwordDto.OldPassword, passwordDto.NewPassword);

                if (!result.Succeeded)
                {
                    var errorResponse = new
                    {
                        status = "fail",
                        result.Errors
                    };
                    return Unauthorized(errorResponse);
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
                    status = "fail",
                    message = "Something went wrong"
                });
            }
        }

    }
}