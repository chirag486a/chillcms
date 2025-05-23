using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos.Account;
using Backend.Dtos.Response;
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
                    var errorResponse = ApiResponse<object>.Error();
                    foreach (var modelState in ModelState)
                    {
                        string key = modelState.Key;
                        var errors = modelState.Value.Errors;
                        foreach (var error in errors)
                        {
                            string errorMessage = error.ErrorMessage;
                            errorResponse.AddError(key, errorMessage);
                        }
                    }
                    return BadRequest(errorResponse);
                }
                var NewUser = createUser.ToUserFromSignupDto();

                // Done to remove editor warning...
                if (NewUser.Email == null) return BadRequest(ApiResponse<object>.Error("Email", "Email not found"));
                var ifExists = await _userManager.FindByEmailAsync(NewUser.Email);
                if (ifExists != null)
                {
                    return BadRequest(ApiResponse<object>.Error("Email", "Email with the user already exits"));
                }


                var result = await _userManager.CreateAsync(NewUser, createUser.Password);
                if (!result.Succeeded)
                {
                    var errorResponse = ApiResponse<object>.Error();
                    foreach (IdentityError item in result.Errors)
                    {
                        Console.WriteLine(item.Code);
                        Console.WriteLine(item.Description);
                        errorResponse.AddError(item.Code, item.Description ?? "");
                    }
                    return BadRequest(errorResponse);
                }
                await _fileService.CreateUserDirectoryAsync(NewUser);

                return Ok(NewUser);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return Problem(err.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = ApiResponse<object>.Error();
                    foreach (var modelState in ModelState)
                    {
                        string key = modelState.Key;
                        var errors = modelState.Value.Errors;
                        foreach (var error in errors)
                        {
                            string errorMessage = error.ErrorMessage;
                            errorResponse.AddError(key, errorMessage);
                        }
                    }
                    return BadRequest(errorResponse);
                }
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                User? loginUser = null;
                if (!string.IsNullOrWhiteSpace(loginDto.Email))
                    loginUser = await _userManager.FindByEmailAsync(loginDto.Email);

                if (loginUser == null)
                {
                    return NotFound(ApiResponse<object>.Error("Email", "Email or password do not match"));
                }

                var results = await _signInManager.CheckPasswordSignInAsync(loginUser, loginDto.Password, false);
                

                if (!results.Succeeded)
                {
                    return Unauthorized(ApiResponse<object>.Error("Password", "Email or password do not match"));
                }


                var role = await _userManager.GetRolesAsync(loginUser);

                var response = new
                {
                    Name = loginUser.Name,
                    Email = loginUser.Email,
                    Role = role,
                    Token = await _tokenService.GenerateTokenAsync(loginUser),
                };
                return Ok(ApiResponse<object>.Success(response));
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(ApiResponse<object>.Error("Server", "Something went wrong"));
            }
        }
        [HttpGet]
        [Authorize]
        public IActionResult IsLoggedIn()
        {
            return Ok(ApiResponse<object?>.Success(null, "User is logged in"));
        }

        [HttpPatch("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto passwordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorResponse = ApiResponse<object>.Error();
                    foreach (var modelState in ModelState)
                    {
                        string key = modelState.Key;
                        var errors = modelState.Value.Errors;
                        foreach (var error in errors)
                        {
                            string errorMessage = error.ErrorMessage;
                            errorResponse.AddError(key, errorMessage);
                        }
                    }
                    return BadRequest(errorResponse);
                }


                var email = passwordDto.Email;

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return Unauthorized(ApiResponse<object>.Error("Email", "Email with the user not registered"));
                }


                var result = await _userManager.ChangePasswordAsync(user, passwordDto.OldPassword, passwordDto.NewPassword);
                // IEnumerable<IdentityError> 'a.Errors { get; }



                if (!result.Succeeded)
                {
                    var errorResponse = ApiResponse<object>.Error();
                    foreach (var error in result.Errors)
                    {
                        errorResponse.AddError(error.Code, error.Description);
                    }
                    return Unauthorized(errorResponse);
                }

                return Ok(ApiResponse<object?>.Success(null, "Password changed successfully"));
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest(ApiResponse<object>.Error("Server", "Something went wrong!"));
            }
        }

    }
}