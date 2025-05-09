using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dtos.Response;
using Backend.Dtos.User;
using Backend.Interfaces.IRepository;
using Backend.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQueryDto queryDto)
        {

            try
            {
                
                var obj = await _userRepository.GetAllUsersAsync(queryDto);

                return Ok(ApiResponse<List<Dictionary<string, object>>>.Success(obj.data, obj.total));

            }
            catch (Exception err)
            {
                return BadRequest(ApiResponse<object>.Error(err.Message));
            }
        }
    }
}