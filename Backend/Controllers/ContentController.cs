using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/content")]
    public class ContentController : ControllerBase
    {
        ApplicationDbContext _context;
        public ContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] ContentMetaCreateDto content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = "fail",
                    errors = ModelState
                });
            }
            return Ok();
        }
    }
}