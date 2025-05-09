using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dtos.Content;
using Backend.Models.Contents;

namespace Backend.Interfaces.IRepository
{
    public interface IContentMetaRepository
    {
        public Task<ContentMeta> CreateContentMetaAsync(ContentMetaCreateDto contentDto, string userId);
        public Task<(List<Dictionary<string, object>> Data, int Total)>  GetContentMetaAsync(GetAllContentMetaQueryDto queryDto);
        public Task<GetContentMetaResponesDto> GetContentMetaByIdAsync(string contentMetaId);
        public Task<GetContentMetaResponesDto> GetContentMetaBySlugAsync(string slug);
        public Task SaveContentFileAsync(ContentFileCreateDto content, string userId);

        public Task<Content> GetContentDetailsAsync(string metaId, string contentId);
        public Task<FileStream> GetContentFile(string metaId, string fileId);

    }
}