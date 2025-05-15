using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

namespace Qase.Csharp.Commons.Core
{
    /// <summary>
    /// Collects information about the host system
    /// </summary>
    public class HostInfo
    {
        /// <summary>
        /// Gets information about the host system
        /// </summary>
        /// <param name="version">The version of the application</param>
        /// <returns>A dictionary containing host information</returns>
        public Dictionary<string, string> GetHostInfo(string? version)
        {
            return new Dictionary<string, string>
            {
                { "name", Dns.GetHostName() },
                { "os", RuntimeInformation.OSDescription },
                { "os_version", Environment.OSVersion.ToString() },
                { "architecture", RuntimeInformation.OSArchitecture.ToString() },
                { "framework", RuntimeInformation.FrameworkDescription },
                { "version", version ?? "unknown" }
            };
        }
    }
} 
