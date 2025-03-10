using System.Drawing;
using System.Net.Mime;
using System.Xml.XPath;
using Backend.Data;
using Backend.Dtos.Content;
using Backend.Dtos.Response;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Interfaces.IRepository;
using Backend.Interfaces.IServices;
using Backend.Mappers;
using Backend.Models.Contents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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
        IContentMetaRepository _contentMetaRepository;
        public ContentController(ApplicationDbContext context, IOptions<FileUploadSettings> options, IFileService fileService, IContentMetaRepository contentMetaRepository)
        {
            _context = context;
            _setting = options.Value;
            _fileService = fileService;
            _contentMetaRepository = contentMetaRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateContentMeta([FromBody] ContentMetaCreateDto content)
        {
            try
            {
                ContentMeta newContent = await _contentMetaRepository.CreateContentMetaAsync(content, User.GetId());
                return Ok(ApiResponse<ContentMetaCreateResponseDto>.Success(
                        newContent.ToContentMetaCreateResponseFromContentMeta(),
                        "Content Meta created successfully")
                    );
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(ApiResponse<ContentMetaCreateResponseDto>.Error(err.Message));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllContentMeta([FromQuery] GetAllContentMetaQueryDto queryDto)
        {

            try
            {
                var results = await _contentMetaRepository.GetContentMetaAsync(queryDto);

                return Ok(ApiResponse<List<ContentMeta>>.Success(results));
            }
            catch (Exception err)
            {
                return BadRequest(ApiResponse<ContentMeta>.Error(err.Message));
            }
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetContentMetaById([FromRoute] string id)
        {
            try
            {
                var results = await _contentMetaRepository.GetContentMetaByIdAsync(id);

                return Ok(ApiResponse<GetContentMetaResponesDto>.Success(results));
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                Console.WriteLine(err.StackTrace);
                return BadRequest(ApiResponse<object>.Error(err.Message));
            }
        }
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetContentMetaBySlug([FromRoute] string slug)
        {
            try
            {
                var results = await _contentMetaRepository.GetContentMetaBySlugAsync(slug);

                return Ok(ApiResponse<GetContentMetaResponesDto>.Success(results));
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                Console.WriteLine(err.StackTrace);
                return BadRequest(ApiResponse<object>.Error(err.Message));
            }
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
        [HttpGet("{metaId:guid}/file/{fileId:guid}")]
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
                    FileDownloadName = Path.GetFileName(content.FileName),
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