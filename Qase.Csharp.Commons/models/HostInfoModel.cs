using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Qase.Csharp.Commons.Models
{
    /// <summary>
    /// Model representing host system information
    /// </summary>
    public class HostInfoModel
    {
        /// <summary>
        /// OS name in lowercase (e.g., "windows", "linux", "darwin")
        /// </summary>
        public string? System { get; set; }

        /// <summary>
        /// Machine hostname
        /// </summary>
        public string MachineName { get; set; } = string.Empty;

        /// <summary>
        /// OS version/release
        /// </summary>
        public string Release { get; set; } = string.Empty;

        /// <summary>
        /// Detailed OS information
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// System architecture
        /// </summary>
        public string Arch { get; set; } = string.Empty;

        /// <summary>
        /// Runtime/language version (e.g., .NET version)
        /// </summary>
        public string? Language { get; set; }

        /// <summary>
        /// Package manager version (e.g., NuGet)
        /// </summary>
        public string? PackageManager { get; set; }

        /// <summary>
        /// Test framework name and version
        /// </summary>
        public string? Framework { get; set; }

        /// <summary>
        /// Reporter version
        /// </summary>
        public string? Reporter { get; set; }

        /// <summary>
        /// Commons library version
        /// </summary>
        public string? Commons { get; set; }

        /// <summary>
        /// API Client V1 version
        /// </summary>
        public string? ApiClientV1 { get; set; }

        /// <summary>
        /// API Client V2 version
        /// </summary>
        public string? ApiClientV2 { get; set; }

        /// <summary>
        /// Converts the model to a dictionary
        /// </summary>
        /// <returns>Dictionary representation of the model</returns>
        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>
            {
                { "machineName", MachineName },
                { "release", Release },
                { "version", Version },
                { "arch", Arch }
            };

            if (!string.IsNullOrWhiteSpace(System))
            {
                dict["system"] = System;
            }

            if (!string.IsNullOrWhiteSpace(Language))
            {
                dict["language"] = Language;
            }

            if (!string.IsNullOrWhiteSpace(PackageManager))
            {
                dict["packageManager"] = PackageManager;
            }

            if (!string.IsNullOrWhiteSpace(Framework))
            {
                dict["framework"] = Framework;
            }

            if (!string.IsNullOrWhiteSpace(Reporter))
            {
                dict["reporter"] = Reporter;
            }

            if (!string.IsNullOrWhiteSpace(Commons))
            {
                dict["commons"] = Commons;
            }

            if (!string.IsNullOrWhiteSpace(ApiClientV1))
            {
                dict["apiClientV1"] = ApiClientV1;
            }

            if (!string.IsNullOrWhiteSpace(ApiClientV2))
            {
                dict["apiClientV2"] = ApiClientV2;
            }

            return dict;
        }

        /// <summary>
        /// Returns a JSON string representation of the host info
        /// </summary>
        /// <returns>JSON string representation</returns>
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(this, options);
        }
    }
}
