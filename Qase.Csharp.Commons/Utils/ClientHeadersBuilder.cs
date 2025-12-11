using System;
using System.Collections.Generic;
using Qase.Csharp.Commons.Core;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Builds X-Client and X-Platform headers for API requests
    /// </summary>
    public static class ClientHeadersBuilder
    {
        /// <summary>
        /// Builds the X-Client header value
        /// </summary>
        /// <param name="reporterName">The name of the reporter (e.g., "qase-xunit")</param>
        /// <param name="reporterVersion">The version of the reporter</param>
        /// <param name="framework">The test framework name (e.g., "xunit", "nunit")</param>
        /// <param name="frameworkVersion">The test framework version</param>
        /// <returns>The X-Client header value</returns>
        public static string BuildXClientHeader(
            string? reporterName = null,
            string? reporterVersion = null,
            string? framework = null,
            string? frameworkVersion = null)
        {
            var parts = new List<string>();

            // Get versions from HostInfo
            var apiClientV1Version = HostInfo.GetApiClientV1Version();
            var apiClientV2Version = HostInfo.GetApiClientV2Version();
            var commonsVersion = HostInfo.GetCommonsVersion();

            // Add reporter information
            if (!string.IsNullOrWhiteSpace(reporterName))
            {
                parts.Add($"reporter={reporterName}");
            }

            if (!string.IsNullOrWhiteSpace(reporterVersion))
            {
                parts.Add($"reporter_version=v{FormatVersion(reporterVersion!)}");
            }

            // Add framework information
            if (!string.IsNullOrWhiteSpace(framework))
            {
                parts.Add($"framework={framework}");
            }

            if (!string.IsNullOrWhiteSpace(frameworkVersion))
            {
                parts.Add($"framework_version={FormatVersion(frameworkVersion!)}");
            }

            // Add client versions
            if (!string.IsNullOrWhiteSpace(apiClientV1Version))
            {
                parts.Add($"client_version_v1=v{FormatVersion(apiClientV1Version!)}");
            }

            if (!string.IsNullOrWhiteSpace(apiClientV2Version))
            {
                parts.Add($"client_version_v2=v{FormatVersion(apiClientV2Version!)}");
            }

            // Add commons version
            if (!string.IsNullOrWhiteSpace(commonsVersion))
            {
                parts.Add($"core_version=v{FormatVersion(commonsVersion!)}");
            }

            return string.Join(";", parts);
        }

        /// <summary>
        /// Builds the X-Platform header value
        /// </summary>
        /// <param name="language">The programming language (default: "csharp")</param>
        /// <param name="languageVersion">The language version</param>
        /// <param name="packageManager">The package manager name (default: "nuget")</param>
        /// <param name="packageManagerVersion">The package manager version</param>
        /// <returns>The X-Platform header value</returns>
        public static string BuildXPlatformHeader(
            string? language = null,
            string? languageVersion = null,
            string? packageManager = null,
            string? packageManagerVersion = null)
        {
            var parts = new List<string>();

            // OS information
            var osName = HostInfo.GetOsName();
            if (!string.IsNullOrWhiteSpace(osName))
            {
                parts.Add($"os={osName}");
            }

            // Architecture
            var arch = HostInfo.GetArchitecture();
            if (!string.IsNullOrWhiteSpace(arch))
            {
                parts.Add($"arch={arch}");
            }

            // Language (default to csharp)
            var lang = !string.IsNullOrWhiteSpace(language) ? language : "csharp";
            var langVer = !string.IsNullOrWhiteSpace(languageVersion) 
                ? languageVersion 
                : HostInfo.GetCSharpVersion();
            
            if (!string.IsNullOrWhiteSpace(langVer))
            {
                parts.Add($"{lang}={langVer}");
            }
            else if (!string.IsNullOrWhiteSpace(lang) && lang != null)
            {
                parts.Add(lang);
            }

            // Package manager (default to nuget)
            var pkgMgr = !string.IsNullOrWhiteSpace(packageManager) ? packageManager : "nuget";
            var pkgMgrVer = !string.IsNullOrWhiteSpace(packageManagerVersion) 
                ? packageManagerVersion 
                : HostInfo.GetNuGetVersion();

            if (!string.IsNullOrWhiteSpace(pkgMgrVer))
            {
                parts.Add($"{pkgMgr}={pkgMgrVer}");
            }
            else if (!string.IsNullOrWhiteSpace(pkgMgr) && pkgMgr != null)
            {
                parts.Add(pkgMgr);
            }

            return string.Join(";", parts);
        }

        /// <summary>
        /// Formats version string (removes 'v' prefix if present, ensures proper format)
        /// </summary>
        private static string FormatVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return string.Empty;
            }

            // Remove 'v' prefix if present
            var formatted = version.TrimStart('v', 'V');

            return formatted;
        }
    }
}
