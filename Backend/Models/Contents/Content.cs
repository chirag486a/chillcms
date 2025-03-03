using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.Contents
{
    public class Content
    {
        public string FileName { get; set; } = string.Empty;
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ContentMetaId { get; set; } = string.Empty;
        public ContentMeta? ContentMeta { get; set; }
        public string Format { get; set; } = string.Empty;
    }
}