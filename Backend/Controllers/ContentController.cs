using System.Drawing;
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
        [HttpGet("meta/{id:guid}")]
        public async Task<IActionResult> GetContentMeta([FromRoute] string id)
        {
            var contentsMeta = await _context.ContentMetas.Select(c => new
            {
                Id = c.Id,
                UserId = c.UserId,
                ContentTitle = c.ContentTitle,
                ContentSlug = c.ContentSlug,
                ContentDescription = c.ContentDescription,
                CreatedAt = c.CreatedAt
            }).FirstOrDefaultAsync(cm => cm.Id == id);
            var contentQuery = _context.Contents.AsQueryable();
            // contentQuery = contentQuery.Include(cm => cm.ContentMeta);
            var contentData = await contentQuery.Where(cm => cm.ContentMetaId == id).Select(c => new
            {
                FileName = c.FileName,
                Id = c.Id,
                Format = c.Format
            }).ToListAsync();
            return Ok(new
            {
                contentsMeta,
                contentData
            });
        }
        [HttpPost("file")]
        [Authorize]
        public async Task<IActionResult> UploadContentFile([FromForm] ContentFileCreateDto content)
        {
            try
            {
                if (content.Files == null || content.Files.Count == 0)
                {
                    return BadRequest("File is required");
                }
                var contentMeta = await _context.ContentMetas.FirstOrDefaultAsync(cm => cm.Id == content.ContentId);
                if (contentMeta is null)
                {
                    return BadRequest(new
                    {
                        status = "fail",
                        message = "Create content meta data first"
                    });
                }
                if (!_setting.AllowedFileTypes.TryGetValue(content.FileType, out var allowedExtension))
                    return BadRequest("Invalid file format");
                foreach (var file in content.Files)
                {
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (!allowedExtension.Contains(fileExtension))
                    {
                        return BadRequest($"Content with type {fileExtension} is not allowed for format {content.FileType}.");
                    }

                    if (file.Length > _setting.MaxFileSize)
                    {
                        return BadRequest("File size exceeds the maximum limits");
                    }
                }

                var UserId = User.GetId();
                await _fileService.CreateFormatDirectory(UserId, content.ContentId, content.FileType);
                int i = 0;
                // Db call
                foreach (Content c in content.ToContentIEnumerableFromContentFileCreateDto())
                {
                    await _context.Contents.AddAsync(c);
                    await _context.SaveChangesAsync();
                    // save file
                    await _fileService.SaveContent(UserId, c, content.Files[i++]);

                }

                return Ok("Ok");
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(err);
            }
        }
        [HttpGet("meta/{metaId:guid}/file/{fileId:guid}")]
        public async Task<IActionResult> GetContentFile([FromRoute] string metaId, [FromRoute] string fileId)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(metaId) || string.IsNullOrWhiteSpace(fileId))
                {
                    return BadRequest("Fuck just send file id or meta id");
                }
                var contentMeta = await _context.ContentMetas.FirstOrDefaultAsync(c => c.Id == metaId);
                if (contentMeta == null)
                {
                    return BadRequest("Why ask the file that does not exits");
                }
                var content = await _context.Contents.FirstOrDefaultAsync(c => c.ContentMetaId == metaId && c.Id == fileId);
                if (content == null)
                {
                    return BadRequest("Why ask the file that does not exits");
                }

                FileStream fileStream = _fileService.GetContent(contentMeta, content);


                return new FileStreamResult(fileStream, "application/octet-stream")
                {
                    FileDownloadName = Path.GetFileName(content.FileName)
                };

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest("Something went wrong");
            }
        }
    }
}