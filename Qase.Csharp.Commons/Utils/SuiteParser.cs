using System.Collections.Generic;
using System.Linq;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Utility class for extracting suite hierarchies from test names.
    /// Provides shared logic for all reporters (xUnit, NUnit, MSTest).
    /// </summary>
    public static class SuiteParser
    {
        /// <summary>
        /// Extracts suite hierarchy from a full type name (namespace + class).
        /// Each dot-separated segment becomes a suite level.
        /// Used by xUnit (ITypeInfo.Name) and MSTest (fullTypeName).
        /// </summary>
        /// <param name="typeName">Full type name, e.g., "Namespace.Sub.ClassName"</param>
        /// <returns>List of SuiteData with one entry per name segment</returns>
        public static List<SuiteData> FromTypeName(string? typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return new List<SuiteData>();

            return typeName.Split('.')
                .Select(part => new SuiteData { Title = part })
                .ToList();
        }

        /// <summary>
        /// Extracts suite hierarchy from a full test name (namespace + class + method + optional parameters).
        /// Strips the method name and any parenthesized parameters, returning only the suite segments.
        /// Used by NUnit (fullname attribute from XML events).
        /// </summary>
        /// <param name="fullTestName">Full test name, e.g., "Ns.Sub.Class.Method(params)"</param>
        /// <returns>List of SuiteData with one entry per namespace/class segment</returns>
        public static List<SuiteData> FromFullTestName(string? fullTestName)
        {
            if (string.IsNullOrEmpty(fullTestName))
                return new List<SuiteData>();

            // Strip parameters BEFORE splitting to prevent dots in parameter
            // values (e.g., "50.0") from creating spurious suite levels
            var openParenIndex = fullTestName.IndexOf('(');
            var nameWithoutParams = openParenIndex > 0
                ? fullTestName.Substring(0, openParenIndex)
                : fullTestName;

            var parts = nameWithoutParams.Split('.');
            if (parts.Length >= 2)
            {
                // All parts except the last (method name) form the suite hierarchy
                return parts.Take(parts.Length - 1)
                    .Select(part => new SuiteData { Title = part })
                    .ToList();
            }

            // Single segment = method name only, no suite hierarchy
            return new List<SuiteData>();
        }
    }
}
