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

        [HttpPost("meta")]
        [Authorize(Policy = "AdminOnly")]
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
                return BadRequest(ApiResponse<ContentMetaCreateResponseDto>.Error("ContentMeta", err.Message));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllContentMeta([FromQuery] GetAllContentMetaQueryDto queryDto)
        {

            try
            {
                var results = await _contentMetaRepository.GetContentMetaAsync(queryDto);

                return Ok(ApiResponse<List<Dictionary<string, object>>>.Success(results.Data, results.Total));
            }
            catch (Exception err)
            {
                return BadRequest(ApiResponse<ContentMeta>.Error("ContentMeta", err.Message));
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
                return BadRequest(ApiResponse<object>.Error("ContentMeta", err.Message));
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
                return BadRequest(ApiResponse<object>.Error("ContentMeta", err.Message));
            }
        }
        [HttpPost("file")]
        [Authorize]
        public async Task<IActionResult> UploadContentFile([FromForm] ContentFileCreateDto content)
        {
            try
            {
                await _contentMetaRepository.SaveContentFileAsync(content, User.GetId());
                return Ok(ApiResponse<object?>.Success(null));
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(ApiResponse<object>.Error("ContentFile", err.Message));

            }
        }
        [HttpGet("{metaId:guid}/{fileId:guid}")]
        public async Task<IActionResult> GetContentFile([FromRoute] string metaId, [FromRoute] string fileId)
        {
            try
            {

                var content = await _contentMetaRepository.GetContentDetailsAsync(metaId, fileId);
                var fileStream = await _contentMetaRepository.GetContentFile(metaId, fileId);


                return new FileStreamResult(fileStream, MimeTypeConverter.GetMimeType(filePath: content.FileName));


            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest(ApiResponse<object>.Error("ContentFile", err.Message));
            }
        }
    }
}