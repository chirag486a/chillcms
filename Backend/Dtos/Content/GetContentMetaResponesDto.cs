using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Backend.Dtos.Content
{
    public class ContentMetaResponseDto
    {

        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ContentTitle { get; set; } = string.Empty;
        public string ContentDescription { get; set; } = string.Empty;
        public string ContentSlug { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
    public class ContentFileInfoResponse
    {
        public string FileName { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;

    }
    public class GetContentMetaResponesDto
    {
        public ContentMetaResponseDto? Meta { get; set; }
        public List<ContentFileInfoResponse> Data { get; set; } = [];

    }
}