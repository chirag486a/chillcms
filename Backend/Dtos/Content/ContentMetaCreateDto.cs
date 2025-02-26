using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos.Content
{
    public class ContentMetaCreateDto
    {
        [Required]
        [MinLength(10, ErrorMessage = "Content title cannot be less than 10 characters")]
        [MaxLength(1024, ErrorMessage = "Content title cannot be more than 1024 characters")]
        public string? ContentTitle { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Content content slug cannot be less than 10 characters")]
        [MaxLength(1024, ErrorMessage = "Content content slug cannot be more than 1024 characters")]
        public string? ContentSlug { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Content description cannot be less than 10 characters")]
        [MaxLength(1024, ErrorMessage = "Content description cannot be more than 1024 characters")]
        public string? ContentDescription { get; set; }

    }
}