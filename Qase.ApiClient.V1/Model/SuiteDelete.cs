// <auto-generated>
/*
 * Qase.io TestOps API v1
 *
 * Qase TestOps API v1 Specification.
 *
 * The version of the OpenAPI document: 1.0.0
 * Contact: support@qase.io
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */

#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Qase.ApiClient.V1.Client;

namespace Qase.ApiClient.V1.Model
{
    /// <summary>
    /// SuiteDelete
    /// </summary>
    public partial class SuiteDelete : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteDelete" /> class.
        /// </summary>
        /// <param name="destinationId">If provided, child test cases would be moved to suite with such ID.</param>
        [JsonConstructor]
        public SuiteDelete(Option<long?> destinationId = default)
        {
            DestinationIdOption = destinationId;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Used to track the state of DestinationId
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<long?> DestinationIdOption { get; private set; }

        /// <summary>
        /// If provided, child test cases would be moved to suite with such ID.
        /// </summary>
        /// <value>If provided, child test cases would be moved to suite with such ID.</value>
        [JsonPropertyName("destination_id")]
        public long? DestinationId { get { return this.DestinationIdOption; } set { this.DestinationIdOption = new Option<long?>(value); } }

        /// <summary>
        /// Gets or Sets additional properties
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; } = new Dictionary<string, JsonElement>();

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class SuiteDelete {\n");
            sb.Append("  DestinationId: ").Append(DestinationId).Append("\n");
            sb.Append("  AdditionalProperties: ").Append(AdditionalProperties).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

    /// <summary>
    /// A Json converter for type <see cref="SuiteDelete" />
    /// </summary>
    public class SuiteDeleteJsonConverter : JsonConverter<SuiteDelete>
    {
        /// <summary>
        /// Deserializes json to <see cref="SuiteDelete" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override SuiteDelete Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            Option<long?> destinationId = default;

            while (utf8JsonReader.Read())
            {
                if (startingTokenType == JsonTokenType.StartObject && utf8JsonReader.TokenType == JsonTokenType.EndObject && currentDepth == utf8JsonReader.CurrentDepth)
                    break;

                if (startingTokenType == JsonTokenType.StartArray && utf8JsonReader.TokenType == JsonTokenType.EndArray && currentDepth == utf8JsonReader.CurrentDepth)
                    break;

                if (utf8JsonReader.TokenType == JsonTokenType.PropertyName && currentDepth == utf8JsonReader.CurrentDepth - 1)
                {
                    string? localVarJsonPropertyName = utf8JsonReader.GetString();
                    utf8JsonReader.Read();

                    switch (localVarJsonPropertyName)
                    {
                        case "destination_id":
                            destinationId = new Option<long?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (long?)null : utf8JsonReader.GetInt64());
                            break;
                        default:
                            break;
                    }
                }
            }

            if (destinationId.IsSet && destinationId.Value == null)
                throw new ArgumentNullException(nameof(destinationId), "Property is not nullable for class SuiteDelete.");

            return new SuiteDelete(destinationId);
        }

        /// <summary>
        /// Serializes a <see cref="SuiteDelete" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="suiteDelete"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, SuiteDelete suiteDelete, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            WriteProperties(writer, suiteDelete, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="SuiteDelete" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="suiteDelete"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, SuiteDelete suiteDelete, JsonSerializerOptions jsonSerializerOptions)
        {
            if (suiteDelete.DestinationIdOption.IsSet)
                writer.WriteNumber("destination_id", suiteDelete.DestinationIdOption.Value!.Value);
        }
    }
}
