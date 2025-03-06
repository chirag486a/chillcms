using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Backend.Dtos.Content;
using Backend.Models;
using Backend.Models.Contents;

namespace Backend.Mappers
{
    public static class ContentMappers
    {
        public static ContentMeta ToContentMetaFromContentMetaCreateDto(this ContentMetaCreateDto metaDto)
        {
            return new ContentMeta
            {
                ContentTitle = metaDto.ContentTitle,
                ContentSlug = metaDto.ContentSlug,
                ContentDescription = metaDto.ContentDescription
            };
        }
        public static ContentMetaCreateResponseDto ToContentMetaCreateResponseFromContentMeta(this ContentMeta meta)
        {
            return new ContentMetaCreateResponseDto
            {
                Id = meta.Id,
                UserId = meta.UserId,
                CreatedAt = meta.CreatedAt,
                ContentTitle = meta.ContentTitle,
                ContentSlug = meta.ContentSlug,
                ContentDescription = meta.ContentDescription
            };
        }
        public static IEnumerable<Content> ToContentIEnumerableFromContentFileCreateDto(this ContentFileCreateDto contentFileCreateDto)
        {
            foreach (var file in contentFileCreateDto.Files)
            {
                yield return new Content
                {
                    Format = contentFileCreateDto.FileType,
                    FileName = file.FileName,
                    ContentMetaId = contentFileCreateDto.ContentId
                };
            }
        }
    }
}