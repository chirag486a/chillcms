using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos.Content;
using Backend.Helpers;
using Backend.Interfaces.IRepository;
using Backend.Interfaces.IServices;
using Backend.Mappers;
using Backend.Models.Contents;
using Backend.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Microsoft.VisualBasic;

namespace Backend.Repositories
{
    public class ContentMetaRepository : IContentMetaRepository
    {
        ApplicationDbContext _context;
        IFileService _fileService;
        FileUploadSettings _setting;
        public ContentMetaRepository(ApplicationDbContext context, IFileService fileService, IOptions<FileUploadSettings> setting)
        {
            _setting = setting.Value;
            _context = context;
            _fileService = fileService;

        }
        public async Task<ContentMeta> CreateContentMetaAsync(ContentMetaCreateDto content, string userId)
        {
            try
            {
                ContentMeta newContent = content.ToContentMetaFromContentMetaCreateDto();
                newContent.UserId = userId;
                var slugExits = await _context.ContentMetas.FirstOrDefaultAsync(c => c.ContentSlug == content.ContentSlug);
                if (slugExits is not null)
                {
                    throw new Exception("Content slug must be unique");
                }

                await _context.ContentMetas.AddAsync(newContent);
                await _context.SaveChangesAsync();
                await _fileService.CreateContentDirectory(userId, newContent);
                return newContent;

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<FileStream> GetContentFile(string metaId, string fileId)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(metaId) || string.IsNullOrWhiteSpace(fileId))
                {
                    throw new Exception("Fuck just send file id or meta id");
                }
                var contentMeta = await _context.ContentMetas.FirstOrDefaultAsync(c => c.Id == metaId);
                if (contentMeta == null)
                {
                    throw new Exception("Why ask the file that does not exits");
                }
                var content = await GetContentDetailsAsync(metaId, fileId);

                FileStream fileStream = _fileService.GetContent(contentMeta, content);

                return fileStream;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<Content> GetContentDetailsAsync(string metaId, string contentId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(metaId) || string.IsNullOrWhiteSpace(contentId))
                {
                    throw new Exception("Fuck just send file id or meta id");
                }
                var content = await _context.Contents.FirstOrDefaultAsync(c => c.Id == contentId && c.ContentMetaId == metaId);

                if (content == null)
                {
                    throw new Exception("Content not found");
                }
                return content;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<List<ContentMeta>> GetContentMetaAsync(GetAllContentMetaQueryDto queryDto)
        {
            try
            {

                var contents = _context.ContentMetas.AsQueryable();

                if (!string.IsNullOrWhiteSpace(queryDto.Id))
                {
                    contents = contents.Where(c => c.Id == queryDto.Id);
                }
                if (!string.IsNullOrWhiteSpace(queryDto.UserId))
                {
                    contents = contents.Where(c => c.UserId == queryDto.UserId);
                }
                contents = contents.Skip((queryDto.Page - 1) * queryDto.PageSize).Take(queryDto.PageSize);


                var sortField = queryDto.SortBy
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                System.Reflection.PropertyInfo? property = null;

                var skip = 0;

                foreach (var item in sortField)
                {
                    property = typeof(ContentMeta).GetProperty(string.IsNullOrWhiteSpace(item) ? "CreatedAt" : item);
                    if (property != null) break;
                }

                List<ContentMeta> results;
                if (property == null)
                {
                    contents = queryDto.IsDescending ? contents.OrderByDescending(c => c.CreatedAt) : contents.OrderBy(c => c.CreatedAt);
                    results = await contents.ToListAsync();
                    return results;
                }



                var param = Expression.Parameter(typeof(ContentMeta), "c");
                var propertyAccess = Expression.Property(param, property);
                var conversion = Expression.Convert(propertyAccess, typeof(object));

                var orderByExp = Expression.Lambda<Func<ContentMeta, object>>(conversion, param);

                contents = queryDto.IsDescending ? contents.OrderByDescending(orderByExp) : contents.OrderBy(orderByExp);


                foreach (var field in sortField.Skip(skip))
                {
                    property = typeof(ContentMeta).GetProperty(field);
                    if (property == null) continue;

                    propertyAccess = Expression.Property(param, property);
                    conversion = Expression.Convert(propertyAccess, typeof(object));
                    orderByExp = Expression.Lambda<Func<ContentMeta, object>>(conversion, param);

                    contents = queryDto.IsDescending
                        ? ((IOrderedQueryable<ContentMeta>)contents).ThenByDescending(orderByExp)
                        : ((IOrderedQueryable<ContentMeta>)contents).ThenBy(orderByExp);
                }

                results = await contents.ToListAsync();
                return results;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }

        public async Task<GetContentMetaResponesDto> GetContentMetaByIdAsync(string contentMetaId)
        {
            try
            {

                var contentMeta = await _context.ContentMetas.Select(c => new ContentMetaResponseDto
                {
                    Id = c.Id,
                    ContentTitle = c.ContentTitle ?? string.Empty,
                    ContentSlug = c.ContentSlug ?? string.Empty,
                    ContentDescription = c.ContentDescription ?? string.Empty,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId
                }).FirstOrDefaultAsync(cm => cm.Id == contentMetaId);

                if (contentMeta is null)
                {
                    throw new Exception("Content meta is null");
                }

                var contentQuery = _context.Contents.AsQueryable();

                var contentData = await contentQuery.Where(cm => cm.ContentMetaId == contentMetaId).Select(c => new ContentFileInfoResponse
                {
                    Id = c.Id,
                    FileName = c.FileName,
                    Format = c.Format
                }).ToListAsync();
                return new GetContentMetaResponesDto
                {
                    Meta = contentMeta,
                    Data = contentData
                };
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }
        public async Task<GetContentMetaResponesDto> GetContentMetaBySlugAsync(string slug)
        {
            var contentMeta = await _context.ContentMetas.Select(c => new ContentMetaResponseDto
            {
                Id = c.Id,
                ContentTitle = c.ContentTitle ?? string.Empty,
                ContentSlug = c.ContentSlug ?? string.Empty,
                ContentDescription = c.ContentDescription ?? string.Empty,
                CreatedAt = c.CreatedAt,
                UserId = c.UserId
            }).FirstOrDefaultAsync(cm => cm.ContentSlug == slug);

            if (contentMeta is null)
            {
                throw new Exception("Content meta is null");
            }

            var contentQuery = _context.Contents.AsQueryable();

            var contentData = await contentQuery.Where(cm => cm.ContentMetaId == contentMeta.Id).Select(c => new ContentFileInfoResponse
            {
                Id = c.Id,
                FileName = c.FileName,
                Format = c.Format
            }).ToListAsync();
            return new GetContentMetaResponesDto
            {
                Meta = contentMeta,
                Data = contentData
            };
        }
        public async Task SaveContentFileAsync(ContentFileCreateDto content, string userId)
        {
            try
            {
                if (content.Files == null || content.Files.Count == 0)
                {
                    throw new Exception("File is required");
                }
                var contentMeta = await _context.ContentMetas.FirstOrDefaultAsync(cm => cm.Id == content.ContentId);
                if (contentMeta is null)
                {
                    throw new Exception("Create content meta data first");
                }
                if (!_setting.AllowedFileTypes.TryGetValue(content.FileType, out var allowedExtension))
                    throw new Exception("Invalid file format");
                foreach (var file in content.Files)
                {
                    Console.WriteLine(file.ContentType);
                    Console.WriteLine(file.Headers);
                    Console.WriteLine(file.ContentDisposition);
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (!allowedExtension.Contains(fileExtension))
                    {
                        throw new Exception($"Content with type {fileExtension} is not allowed for format {content.FileType}.");
                    }

                    if (file.Length > _setting.MaxFileSize)
                    {
                        throw new Exception("File size exceeds the maximum limits");
                    }
                }

                await _fileService.CreateFormatDirectory(userId, content.ContentId, content.FileType);
                int i = 0;
                // Db call
                foreach (Content c in content.ToContentIEnumerableFromContentFileCreateDto())
                {
                    await _context.Contents.AddAsync(c);
                    await _context.SaveChangesAsync();
                    // save file
                    await _fileService.SaveContent(userId, c, content.Files[i++]);

                }

                return;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }
    }
}