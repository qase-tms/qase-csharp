using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qase.Csharp.Commons.Serialization
{
    /// <summary>
    /// Converts enum values to and from lowercase string representations for JSON serialization.
    /// Required for Qase Report specification compliance, which expects lowercase enum strings
    /// (e.g., "passed", "failed", "skipped") instead of PascalCase or numeric values.
    /// </summary>
    /// <typeparam name="T">The enum type to convert</typeparam>
    public class LowercaseEnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        /// <summary>
        /// Reads and converts the JSON string to an enum value.
        /// Performs case-insensitive parsing to handle both lowercase and PascalCase inputs.
        /// </summary>
        /// <param name="reader">The JSON reader</param>
        /// <param name="typeToConvert">The target enum type</param>
        /// <param name="options">The serializer options</param>
        /// <returns>The parsed enum value</returns>
        /// <exception cref="JsonException">Thrown when the value cannot be parsed to the target enum type</exception>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string enumString = reader.GetString();
                if (string.IsNullOrEmpty(enumString))
                {
                    throw new JsonException($"Cannot convert empty string to {typeof(T).Name}");
                }

                // Case-insensitive parsing to handle both lowercase and PascalCase
                if (Enum.TryParse<T>(enumString, ignoreCase: true, out var result))
                {
                    return result;
                }

                throw new JsonException($"Unable to convert \"{enumString}\" to {typeof(T).Name}");
            }

            throw new JsonException($"Unexpected token type {reader.TokenType} when parsing {typeof(T).Name}");
        }

        /// <summary>
        /// Writes the enum value as a lowercase JSON string.
        /// Converts the enum name to lowercase per Qase Report specification.
        /// </summary>
        /// <param name="writer">The JSON writer</param>
        /// <param name="value">The enum value to write</param>
        /// <param name="options">The serializer options</param>
        /// <exception cref="JsonException">Thrown when the enum value cannot be converted to its name</exception>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            string enumName = Enum.GetName(typeof(T), value);
            if (enumName == null)
            {
                throw new JsonException($"Cannot get name for enum value {value} of type {typeof(T).Name}");
            }

            writer.WriteStringValue(enumName.ToLowerInvariant());
        }
    }
}
