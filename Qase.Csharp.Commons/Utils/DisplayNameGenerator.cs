#nullable enable

using System.Collections.Generic;
using System.Linq;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Generates display names for ContextManager lookup keys.
    /// </summary>
    public static class DisplayNameGenerator
    {
        /// <summary>
        /// Generates a display name from a base name and optional parameters.
        /// Format: "BaseName" or "BaseName(param1: value1, param2: value2)"
        /// </summary>
        public static string Generate(string baseName, Dictionary<string, string>? parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return baseName;

            var parameterStrings = parameters.Select(kvp => $"{kvp.Key}: {kvp.Value}");
            return $"{baseName}({string.Join(", ", parameterStrings)})";
        }
    }
}
