using System.Collections.Generic;
using System.IO;

namespace Qase.Csharp.Commons.Core
{
    /// <summary>
    /// Provides MIME type detection based on file extension
    /// </summary>
    internal static class MimeTypes
    {
        private static readonly Dictionary<string, string> ExtensionMap = new Dictionary<string, string>
        {
            // Images
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".svg", "image/svg+xml" },
            { ".webp", "image/webp" },
            { ".ico", "image/x-icon" },

            // Text
            { ".txt", "text/plain" },
            { ".log", "text/plain" },
            { ".csv", "text/csv" },
            { ".html", "text/html" },
            { ".htm", "text/html" },
            { ".css", "text/css" },
            { ".xml", "application/xml" },

            // Application
            { ".json", "application/json" },
            { ".pdf", "application/pdf" },
            { ".zip", "application/zip" },
            { ".gz", "application/gzip" },
            { ".tar", "application/x-tar" },
            { ".js", "application/javascript" },

            // Video
            { ".mp4", "video/mp4" },
            { ".webm", "video/webm" },
            { ".avi", "video/x-msvideo" },
        };

        /// <summary>
        /// Guesses the MIME type from a file name based on its extension.
        /// Returns "application/octet-stream" if the type cannot be determined.
        /// </summary>
        public static string GuessFromFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "application/octet-stream";
            }

            var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
            if (ext != null && ExtensionMap.TryGetValue(ext, out var mimeType))
            {
                return mimeType;
            }

            return "application/octet-stream";
        }
    }
}
