using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Extracts parameter values from test display name strings and maps them
    /// to method parameter names. Handles both MSTest format ("Method (a,b)")
    /// and NUnit format ("Ns.Class.Method(\"a\",\"b\")").
    /// </summary>
    public static class ParameterParser
    {
        /// <summary>
        /// Parses parameter values from a test display name string.
        /// Finds the parenthesized section and extracts comma-separated values
        /// with quote-aware splitting and backslash-escape handling.
        /// </summary>
        /// <param name="displayName">The test display name (e.g., "Method (val1,val2)" or "Ns.Class.Method(\"val1\")")</param>
        /// <returns>List of parsed string values, or empty list if no parameters found</returns>
        public static List<string> ParseValues(string? displayName)
        {
            var parameters = new List<string>();
            if (string.IsNullOrEmpty(displayName))
                return parameters;

            var openParenIndex = displayName.IndexOf('(');
            var closeParenIndex = displayName.LastIndexOf(')');

            if (openParenIndex < 0 || closeParenIndex <= openParenIndex)
                return parameters;

            var paramsString = displayName
                .Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1)
                .Trim();

            if (string.IsNullOrEmpty(paramsString))
                return parameters;

            var currentParam = new StringBuilder();
            bool inQuotes = false;
            bool escapeNext = false;

            for (int i = 0; i < paramsString.Length; i++)
            {
                char ch = paramsString[i];

                if (escapeNext)
                {
                    if (ch == '"' || ch == '\\')
                        currentParam.Append(ch);
                    else
                    {
                        currentParam.Append('\\');
                        currentParam.Append(ch);
                    }
                    escapeNext = false;
                    continue;
                }

                if (ch == '\\') { escapeNext = true; continue; }
                if (ch == '"') { inQuotes = !inQuotes; continue; }

                if (ch == ',' && !inQuotes)
                {
                    parameters.Add(currentParam.ToString().Trim());
                    currentParam.Clear();
                    continue;
                }

                currentParam.Append(ch);
            }

            // Always add the last parameter (even if empty string from quoted "")
            parameters.Add(currentParam.ToString().Trim());

            return parameters;
        }

        /// <summary>
        /// Parses parameter values from a display name and maps them to method parameter names.
        /// Uses MethodInfo.GetParameters() to get parameter names and zips them with parsed values.
        /// </summary>
        /// <param name="displayName">The test display name containing parameter values</param>
        /// <param name="methodInfo">The resolved MethodInfo for parameter name lookup</param>
        /// <returns>Dictionary mapping parameter names to their string values, or empty dictionary</returns>
        public static Dictionary<string, string> ParseAndMap(
            string? displayName, MethodInfo? methodInfo)
        {
            var result = new Dictionary<string, string>();
            if (methodInfo == null)
                return result;

            var values = ParseValues(displayName);
            if (values.Count == 0)
                return result;

            var methodParams = methodInfo.GetParameters();
            for (int i = 0; i < Math.Min(values.Count, methodParams.Length); i++)
            {
                var paramName = methodParams[i].Name ?? $"param{i}";
                result[paramName] = string.IsNullOrEmpty(values[i]) ? "empty" : values[i];
            }

            return result;
        }
    }
}
