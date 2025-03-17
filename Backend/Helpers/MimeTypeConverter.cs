using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers;
public class MimeTypeConverter
{
    private static readonly Dictionary<string, string> _mimeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { ".txt", "text/plain" },
            { ".pdf", "application/pdf" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".csv", "text/csv" },
            { ".xml", "application/xml" },
            { ".json", "application/json" },
            { ".zip", "application/zip" },
            { ".mp3", "audio/mpeg" },
            { ".mp4", "video/mp4" },
        };
    public static string GetMimeType(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return "application/octet-stream";
        }
        string extension = Path.GetExtension(filePath).ToLowerInvariant();

        if (_mimeTypes.TryGetValue(extension, out string? mimeType))
        {
            return mimeType;
        }
        return "application/octet-stream";
    }
}