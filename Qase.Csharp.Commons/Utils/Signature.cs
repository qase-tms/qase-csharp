using System.Collections.Generic;
using System.Linq;

namespace Qase.Csharp.Commons.Utils
{
    /// <summary>
    /// Utility class for generating signatures for test cases
    /// </summary>
    public static class Signature
    {
        /// <summary>
        /// Generates a signature for a test case
        /// </summary>
        /// <param name="ids">Test case IDs</param>
        /// <param name="suites">Test case suites</param>
        /// <param name="parameters">Test case parameters</param>
        /// <returns>Signature</returns>
        public static string Generate(IEnumerable<long>? ids, IEnumerable<string?>? suites, IDictionary<string, string>? parameters)
        {
            var parts = new List<string>();

            if (ids != null && ids.Count() > 0)
            {
                parts.Add(string.Join("-", ids.Select(id => id.ToString())));
            }

            if (suites != null && suites.Count() > 0)
            {
                foreach (var suite in suites)
                {
                    if (suite != null)
                    {
                        parts.Add(suite.ToLower().Trim().Replace(" ", "-"));
                    }
                }
            }

            if (parameters != null && parameters.Count() > 0)
            {
                foreach (var parameter in parameters)
                {
                    parts.Add($"\"{parameter.Key.ToLower().Trim()}\":\"{parameter.Value.ToLower().Trim()}\"");
                }
            }

            return string.Join("::", parts);
        }
    }
}
