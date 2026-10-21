using System;
using System.Collections.Generic;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Resolves the Qase web app URL from the <c>testops.api.host</c> option
    /// </summary>
    public static class AppUrlResolver
    {
        private const string DefaultHost = "qase.io";

        // Enterprise hosts share the api-<name>.qase.io shape but serve the app on <name>.qase.io,
        // so a cluster cannot be inferred from the api- prefix and has to be listed explicitly.
        private static readonly Dictionary<string, string> ClusterAppHosts =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["api-eu.qase.io"] = "app-eu.qase.io",
            };

        /// <summary>
        /// Returns the web app base URL, with a trailing slash, for the given API host
        /// </summary>
        /// <param name="host">The configured API host, e.g. <c>qase.io</c>, <c>eu.qase.io</c> or <c>example.qase.io</c></param>
        public static string Resolve(string? host)
        {
            if (string.IsNullOrEmpty(host) || host == DefaultHost)
            {
                return "https://app.qase.io/";
            }

            // Mirrors how the API base URL is built from the host: https://api-<host>.
            if (ClusterAppHosts.TryGetValue($"api-{host}", out var appHost))
            {
                return $"https://{appHost}/";
            }

            return $"https://{host}/";
        }
    }
}
