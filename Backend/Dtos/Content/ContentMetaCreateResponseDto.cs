using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos.Content
{
    public class ContentMetaCreateResponseDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;
        public string? ContentTitle { get; set; }
        public string? ContentSlug { get; set; }
        public string? ContentDescription { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}