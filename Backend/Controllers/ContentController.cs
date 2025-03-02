using Backend.Data;
using Backend.Dtos.Content;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Interfaces.IServices;
using Backend.Mappers;
using Backend.Models.Contents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/content")]
    public class ContentController : ControllerBase
    {
        ApplicationDbContext _context;
        FileUploadSettings _setting;
        IFileService _fileService;
        public ContentController(ApplicationDbContext context, IOptions<FileUploadSettings> options, IFileService fileService)
        {
            _context = context;
            _setting = options.Value;
            _fileService = fileService;
        }

        [HttpPost("meta")]
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
            await _fileService.CreateContentDirectory(User.GetId(), newContent);
            return Ok(new { status = "success", message = "Content Meta created successfully" });
        }
        [HttpGet("meta")]
        public async Task<IActionResult> GetAllContentMeta()
        {
            var contents = await _context.ContentMetas.ToListAsync();
            return Ok(contents);
        }
        [HttpPost("file")]
        [Authorize]
        public async Task<IActionResult> UploadContentFile([FromForm] ContentFileCreateDto content)
        {
            if (content.File == null || content.File.Length == 0)
            {
                return BadRequest("File is required");
            }
            Console.WriteLine(content.FileType);
            if (!_setting.AllowedFileTypes.TryGetValue(content.FileType, out var allowedExtension))
                return BadRequest("Invalid file format");
            var fileExtension = Path.GetExtension(content.File.FileName).ToLower();

            if (!allowedExtension.Contains(fileExtension))
                return BadRequest($"File type {fileExtension} is not allowed for format {content.FileType}.");

            if (content.File.Length > _setting.MaxFileSize)
                return BadRequest("File size exceeds the maximum limits");
            
            return Ok("Ok");
        }
    }
}