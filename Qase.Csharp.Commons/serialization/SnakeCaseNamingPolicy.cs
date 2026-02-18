using System.Text;
using System.Text.Json;

namespace Qase.Csharp.Commons.Serialization
{
    /// <summary>
    /// Converts PascalCase property names to snake_case for JSON serialization.
    /// Custom implementation required for netstandard2.0 compatibility,
    /// as JsonNamingPolicy.SnakeCaseLower is only available in .NET 8+.
    /// </summary>
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        /// <summary>
        /// Converts the specified name according to the snake_case naming policy.
        /// </summary>
        /// <param name="name">The name to convert</param>
        /// <returns>The converted name in snake_case format</returns>
        /// <remarks>
        /// Conversion rules:
        /// - Inserts underscore before uppercase letters (except first character)
        /// - Converts entire result to lowercase
        /// - Handles consecutive uppercase letters (e.g., HTTPSConnection -> https_connection)
        /// - Examples: TestOpsId -> test_ops_id, StartTime -> start_time, Id -> id
        /// </remarks>
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            if (name.Length == 1)
            {
                return name.ToLowerInvariant();
            }

            var builder = new StringBuilder();
            builder.Append(char.ToLowerInvariant(name[0]));

            for (int i = 1; i < name.Length; i++)
            {
                char currentChar = name[i];
                char previousChar = name[i - 1];

                if (char.IsUpper(currentChar))
                {
                    // Insert underscore before uppercase letter if:
                    // 1. Previous character is lowercase
                    // 2. OR previous character is uppercase AND next character is lowercase (handles acronyms like HTTPSConnection)
                    if (char.IsLower(previousChar) ||
                        (char.IsUpper(previousChar) && i < name.Length - 1 && char.IsLower(name[i + 1])))
                    {
                        builder.Append('_');
                    }

                    builder.Append(char.ToLowerInvariant(currentChar));
                }
                else
                {
                    builder.Append(currentChar);
                }
            }

            return builder.ToString();
        }
    }
}
