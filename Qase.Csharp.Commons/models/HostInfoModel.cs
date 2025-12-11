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
        /// C#/.NET version (equivalent to "java" in Java version)
        /// </summary>
        public string? Csharp { get; set; }

        /// <summary>
        /// Build tool version (NuGet in C#, equivalent to Maven/Gradle in Java)
        /// </summary>
        public string? BuildTool { get; set; }

        /// <summary>
        /// Reporter version
        /// </summary>
        public string? Reporter { get; set; }

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

            if (!string.IsNullOrWhiteSpace(Csharp))
            {
                dict["csharp"] = Csharp;
            }

            if (!string.IsNullOrWhiteSpace(BuildTool))
            {
                dict["buildTool"] = BuildTool;
            }

            if (!string.IsNullOrWhiteSpace(Reporter))
            {
                dict["reporter"] = Reporter;
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
