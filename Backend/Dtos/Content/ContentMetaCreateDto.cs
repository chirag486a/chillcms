using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos.Content
{
    public class ContentMetaCreateDto
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }

        [Required]
        public string ContentTitle { get; set; }

        [Required]
        public string ContentSlug { get; set; }

        [Required]
        public string ContentDescription { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}