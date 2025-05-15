using System;

namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Configuration for local settings
    /// </summary>
    public class LocalConfig
    {
        /// <summary>
        /// Gets or sets the path to the local report
        /// </summary>
        public string? Path { get; set; } = "./build/qase-report";

        /// <summary>
        /// Gets or sets the local file format
        /// </summary>
        public Format Format { get; set; } = Format.Json;

        public void SetFormat(string? format)
        {
            if (string.IsNullOrEmpty(format) || format == "json")
            {
                Format = Format.Json;
            }
            else if (format == "jsonp")
            {
                Format = Format.Jsonp;
            }
            else
            {
                throw new ArgumentException("Invalid format");
            }
        }
    }
}
