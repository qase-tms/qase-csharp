#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Generates display names for ContextManager lookup keys.
    /// </summary>
    public static class DisplayNameGenerator
    {
        // NUnit renders typed literals with a suffix: 29.99d, 10m, 5L.
        private static readonly Regex NumericLiteralSuffix =
            new Regex(@"^([-+]?\d+(?:\.\d+)?(?:[eE][-+]?\d+)?)(?:[dDfFmMuUlL]{1,2})$", RegexOptions.Compiled);

        /// <summary>
        /// Generates a display name from a base name and optional parameters.
        /// Format: "BaseName" or "BaseName(param1: value1, param2: value2)"
        /// </summary>
        public static string Generate(string baseName, Dictionary<string, string>? parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return baseName;

            var parameterStrings = parameters.Select(kvp => $"{kvp.Key}: {NormalizeValue(kvp.Value)}");
            return $"{baseName}({string.Join(", ", parameterStrings)})";
        }

        /// <summary>
        /// Brings a parameter value rendered by any test framework to one canonical form,
        /// so the value captured at runtime and the one parsed from a display name agree.
        /// </summary>
        public static string NormalizeValue(string? value)
        {
            if (value == null)
                return "null";

            var trimmed = value.Trim();
            if (trimmed.Length >= 2 &&
                ((trimmed[0] == '"' && trimmed[trimmed.Length - 1] == '"') ||
                 (trimmed[0] == '\'' && trimmed[trimmed.Length - 1] == '\'')))
            {
                trimmed = trimmed.Substring(1, trimmed.Length - 2);
            }

            if (trimmed.Length == 0)
                return "empty";

            var match = NumericLiteralSuffix.Match(trimmed);
            if (match.Success)
                trimmed = match.Groups[1].Value;

            // xUnit v3 prints doubles round-trip (29.989999999999998); 15 digits give back 29.99.
            if (trimmed.Length > 15 && trimmed.IndexOf('.') >= 0 &&
                double.TryParse(trimmed, NumberStyles.Float, CultureInfo.InvariantCulture, out var number))
            {
                return number.ToString("G15", CultureInfo.InvariantCulture);
            }

            return trimmed;
        }

        /// <summary>
        /// Renders a runtime argument the way test frameworks show it in display names.
        /// </summary>
        public static string FormatArgument(object? argument)
        {
            switch (argument)
            {
                case null:
                    return "null";
                case string s:
                    return NormalizeValue(s);
                case bool b:
                    return b ? "True" : "False";
                case Enum e:
                    return e.ToString();
                case IFormattable formattable:
                    return NormalizeValue(formattable.ToString(null, CultureInfo.InvariantCulture));
                default:
                    return NormalizeValue(argument.ToString());
            }
        }
    }
}
