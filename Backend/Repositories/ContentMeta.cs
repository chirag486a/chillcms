using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dtos.Content;
using Backend.Interfaces.IRepository;
using Backend.Interfaces.IServices;
using Backend.Mappers;
using Backend.Models.Contents;
using Backend.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ContentMetaRepository : IContentMetaRepository
    {
        ApplicationDbContext _context;
        IFileService _fileService;
        public ContentMetaRepository(ApplicationDbContext context, IFileService fileService)
        {
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
                if (queryDto.IsDescending)
                {
                    contents = contents.OrderByDescending(c => c.CreatedAt);
                }
                var results = await contents.ToListAsync();
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
    }
}