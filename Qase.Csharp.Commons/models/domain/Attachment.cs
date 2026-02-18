using System;
using System.Text.Json.Serialization;

namespace Qase.Csharp.Commons.Models.Domain
{
    /// <summary>
    /// Represents a file attachment
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Gets or sets the attachment ID
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Gets or sets the MIME type
        /// </summary>
        public string? MimeType { get; set; }

        /// <summary>
        /// Gets or sets the content as string
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets the content as bytes
        /// </summary>
        [JsonIgnore]
        public byte[]? ContentBytes { get; set; }

        /// <summary>
        /// Gets or sets the file path
        /// </summary>
        public string? FilePath { get; set; }
    }
} 
