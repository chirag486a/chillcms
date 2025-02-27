using Backend.Data;
using Backend.Dtos.Content;
using Backend.Extensions;
using Backend.Mappers;
using Backend.Models.Contents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> CreateContentMeta([FromBody] ContentMetaCreateDto content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = "fail",
                    errors = ModelState
                });
            }
            ContentMeta newContent = content.ToContentMetaFromContentMetaCreateDto();
            newContent.UserId = User.GetId();

            await _context.ContentMetas.AddAsync(newContent);
            await _context.SaveChangesAsync();
            return Ok(new { status = "success", message = "Content Meta created successfully" });
        }
    }
}