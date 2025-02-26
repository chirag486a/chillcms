using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}