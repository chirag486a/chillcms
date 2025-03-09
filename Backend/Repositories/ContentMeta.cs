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
        public async Task<ContentMeta> CreateContentMeta(ContentMetaCreateDto content, string userId)
        {
            ContentMeta newContent = content.ToContentMetaFromContentMetaCreateDto();
            newContent.UserId = userId;

            await _context.ContentMetas.AddAsync(newContent);
            await _context.SaveChangesAsync();
            await _fileService.CreateContentDirectory(userId, newContent);
            return newContent;
        }
    }
}