using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos.Content
{
    public class ContentFileCreateDto
    {
        [Required]
        public required List<IFormFile> Files { get; set; }

        [Required]
        public string ContentId { get; set; } = string.Empty;

        [Required]
        public string FileType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}