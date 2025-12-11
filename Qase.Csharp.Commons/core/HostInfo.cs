using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using Qase.Csharp.Commons.Models;

namespace Qase.Csharp.Commons.Core
{
    /// <summary>
    /// Collects information about the host system
    /// </summary>
    public static class HostInfo
    {
        /// <summary>
        /// Gets comprehensive information about the host system, including all versions
        /// Structure matches Java implementation for consistency
        /// </summary>
        /// <param name="version">The version of the application (currently unused, kept for compatibility)</param>
        /// <returns>HostInfoModel containing host information including all detected versions</returns>
        public static HostInfoModel GetHostInfo(string? version = null)
        {
            var model = new HostInfoModel
            {
                // machineName - hostname
                MachineName = Dns.GetHostName(),

                // release - OS version
                Release = Environment.OSVersion.ToString(),

                // version - detailed OS information
                Version = RuntimeInformation.OSDescription,

                // arch - architecture
                Arch = RuntimeInformation.OSArchitecture.ToString()
            };

            // system - OS name in lowercase (e.g., "windows", "linux", "macos")
            var osName = GetOsName();
            if (!string.IsNullOrWhiteSpace(osName))
            {
                model.System = osName!.ToLowerInvariant();
            }

            // csharp - C#/.NET version (equivalent to "java" in Java version)
            var csharpVersion = GetCSharpVersion();
            if (!string.IsNullOrWhiteSpace(csharpVersion))
            {
                model.Csharp = csharpVersion;
            }

            // buildTool - build tool version (NuGet in C#, equivalent to Maven/Gradle in Java)
            var buildToolVersion = GetNuGetVersion();
            if (!string.IsNullOrWhiteSpace(buildToolVersion))
            {
                model.BuildTool = buildToolVersion;
            }

            // reporter - reporter version
            var (reporterName, reporterVersion) = DetectReporter();
            if (!string.IsNullOrWhiteSpace(reporterVersion))
            {
                model.Reporter = reporterVersion;
            }

            return model;
        }

        /// <summary>
        /// Gets OS name in a simplified format (e.g., "Windows", "Linux", "macOS")
        /// </summary>
        /// <returns>OS name or null if cannot be determined</returns>
        public static string? GetOsName()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "windows";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "linux";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return "darwin";
                }
            }
            catch
            {
                // Fallback to OSDescription
            }

            // Fallback to OSDescription, but simplify it
            var osDescription = RuntimeInformation.OSDescription;
            if (!string.IsNullOrWhiteSpace(osDescription))
            {
                // Extract OS name from description (e.g., "Linux 5.10.0" -> "Linux")
                var parts = osDescription.Split(' ');
                if (parts.Length > 0)
                {
                    return parts[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the OS architecture
        /// </summary>
        /// <returns>OS architecture as string</returns>
        public static string GetArchitecture()
        {
            return RuntimeInformation.OSArchitecture.ToString();
        }

        /// <summary>
        /// Gets C#/.NET framework version
        /// </summary>
        /// <returns>Framework version or null if cannot be determined</returns>
        public static string? GetCSharpVersion()
        {
            try
            {
                // Try to get from environment or runtime
                // For .NET, we can use RuntimeInformation.FrameworkDescription
                var frameworkDescription = RuntimeInformation.FrameworkDescription;
                if (!string.IsNullOrWhiteSpace(frameworkDescription))
                {
                    // Extract version from framework description (e.g., ".NET 6.0.0" -> "6.0.0")
                    var parts = frameworkDescription.Split(' ');
                    if (parts.Length > 1)
                    {
                        return parts[parts.Length - 1];
                    }
                }
            }
            catch
            {
                // Ignore errors
            }

            return null;
        }

        /// <summary>
        /// Gets NuGet version (if available)
        /// </summary>
        /// <returns>NuGet version or null if not available</returns>
        public static string? GetNuGetVersion()
        {
            // NuGet version is typically not available at runtime
            // This would require checking installed NuGet CLI or package manager version
            // For now, return null to skip this parameter
            return null;
        }

        /// <summary>
        /// Gets the assembly version for a given assembly name
        /// </summary>
        /// <param name="assemblyName">The name of the assembly</param>
        /// <returns>Assembly version as string or null if not found</returns>
        public static string? GetAssemblyVersion(string assemblyName)
        {
            try
            {
                var assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name == assemblyName);

                if (assembly != null)
                {
                    var version = assembly.GetName().Version;
                    if (version != null)
                    {
                        return version.ToString();
                    }
                }
            }
            catch
            {
                // Ignore errors when getting assembly version
            }

            return null;
        }

        /// <summary>
        /// Gets the API Client V1 version
        /// </summary>
        /// <returns>Version string or null if not found</returns>
        public static string? GetApiClientV1Version()
        {
            return GetAssemblyVersion("Qase.ApiClient.V1");
        }

        /// <summary>
        /// Gets the API Client V2 version
        /// </summary>
        /// <returns>Version string or null if not found</returns>
        public static string? GetApiClientV2Version()
        {
            return GetAssemblyVersion("Qase.ApiClient.V2");
        }

        /// <summary>
        /// Gets the Commons library version
        /// </summary>
        /// <returns>Version string or null if not found</returns>
        public static string? GetCommonsVersion()
        {
            return GetAssemblyVersion("Qase.Csharp.Commons");
        }

        /// <summary>
        /// Attempts to detect the test framework from loaded assemblies
        /// </summary>
        /// <returns>Tuple containing framework name and version, or (null, null) if not detected</returns>
        public static (string? framework, string? version) DetectTestFramework()
        {
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                // Check for xUnit
                var xunitAssembly = assemblies.FirstOrDefault(a => 
                    a.GetName().Name != null && 
                    a.GetName().Name.StartsWith("xunit", StringComparison.OrdinalIgnoreCase));
                
                if (xunitAssembly != null)
                {
                    var version = xunitAssembly.GetName().Version?.ToString();
                    return ("xunit", version);
                }

                // Check for NUnit
                var nunitAssembly = assemblies.FirstOrDefault(a => 
                    a.GetName().Name != null && 
                    a.GetName().Name.StartsWith("nunit", StringComparison.OrdinalIgnoreCase));
                
                if (nunitAssembly != null)
                {
                    var version = nunitAssembly.GetName().Version?.ToString();
                    return ("nunit", version);
                }

                // Check for MSTest
                var mstestAssembly = assemblies.FirstOrDefault(a => 
                {
                    var name = a.GetName().Name;
                    return name != null && 
                        (name.IndexOf("MSTest", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         name.IndexOf("Microsoft.VisualStudio.TestPlatform", StringComparison.OrdinalIgnoreCase) >= 0);
                });
                
                if (mstestAssembly != null)
                {
                    var version = mstestAssembly.GetName().Version?.ToString();
                    return ("mstest", version);
                }
            }
            catch
            {
                // Ignore errors
            }

            return (null, null);
        }

        /// <summary>
        /// Attempts to detect the reporter name from loaded assemblies
        /// </summary>
        /// <returns>Tuple containing reporter name and version, or (null, null) if not detected</returns>
        public static (string? reporterName, string? version) DetectReporter()
        {
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                // Check for Qase.XUnit.Reporter
                var xunitReporterAssembly = assemblies.FirstOrDefault(a => 
                    a.GetName().Name == "Qase.XUnit.Reporters" || 
                    a.GetName().Name == "Qase.XUnit.Reporter");
                
                if (xunitReporterAssembly != null)
                {
                    var version = xunitReporterAssembly.GetName().Version?.ToString();
                    return ("qase-xunit", version);
                }

                // Add more reporter detection logic here as needed
            }
            catch
            {
                // Ignore errors
            }

            return (null, null);
        }
    }
} 
