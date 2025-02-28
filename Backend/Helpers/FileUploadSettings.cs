using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class FileUploadSettings
    {
        public Dictionary<string, string[]> AllowedFileTypes { get; set; } = new();
        public long MaxFileSize { get; set; }
    }
}