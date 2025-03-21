using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos.Content
{
    public class GetAllContentMetaQueryDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public bool IsDescending { get; set; } = true;
        public List<string> ShortBy { get; set; } = new List<string>();
        public bool IncludeFiles { get; set; }
    }
}