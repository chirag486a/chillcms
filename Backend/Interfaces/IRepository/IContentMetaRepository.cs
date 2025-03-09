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
        public Task<ContentMeta> CreateContentMeta(ContentMetaCreateDto contentDto, string userId);
    }
}