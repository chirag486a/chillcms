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
        public required IFormFile File { get; set; }

        [Required]
        public int ContentId { get; set; }

        [Required]
        public string Filename { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}