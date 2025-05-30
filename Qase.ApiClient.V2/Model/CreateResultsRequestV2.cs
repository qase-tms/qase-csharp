// <auto-generated>
/*
 * Qase.io TestOps API v2
 *
 * Qase TestOps API v2 Specification.
 *
 * The version of the OpenAPI document: 2.0.0
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
using Qase.ApiClient.V2.Client;

namespace Qase.ApiClient.V2.Model
{
    /// <summary>
    /// CreateResultsRequestV2
    /// </summary>
    public partial class CreateResultsRequestV2 : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResultsRequestV2" /> class.
        /// </summary>
        /// <param name="results">results</param>
        [JsonConstructor]
        public CreateResultsRequestV2(Option<List<ResultCreate>?> results = default)
        {
            ResultsOption = results;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Used to track the state of Results
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<List<ResultCreate>?> ResultsOption { get; private set; }

        /// <summary>
        /// Gets or Sets Results
        /// </summary>
        [JsonPropertyName("results")]
        public List<ResultCreate>? Results { get { return this.ResultsOption; } set { this.ResultsOption = new Option<List<ResultCreate>?>(value); } }

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
            sb.Append("class CreateResultsRequestV2 {\n");
            sb.Append("  Results: ").Append(Results).Append("\n");
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
    /// A Json converter for type <see cref="CreateResultsRequestV2" />
    /// </summary>
    public class CreateResultsRequestV2JsonConverter : JsonConverter<CreateResultsRequestV2>
    {
        /// <summary>
        /// Deserializes json to <see cref="CreateResultsRequestV2" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override CreateResultsRequestV2 Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            Option<List<ResultCreate>?> results = default;

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
                        case "results":
                            results = new Option<List<ResultCreate>?>(JsonSerializer.Deserialize<List<ResultCreate>>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (results.IsSet && results.Value == null)
                throw new ArgumentNullException(nameof(results), "Property is not nullable for class CreateResultsRequestV2.");

            return new CreateResultsRequestV2(results);
        }

        /// <summary>
        /// Serializes a <see cref="CreateResultsRequestV2" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="createResultsRequestV2"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, CreateResultsRequestV2 createResultsRequestV2, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            WriteProperties(writer, createResultsRequestV2, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="CreateResultsRequestV2" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="createResultsRequestV2"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, CreateResultsRequestV2 createResultsRequestV2, JsonSerializerOptions jsonSerializerOptions)
        {
            if (createResultsRequestV2.ResultsOption.IsSet && createResultsRequestV2.Results == null)
                throw new ArgumentNullException(nameof(createResultsRequestV2.Results), "Property is required for class CreateResultsRequestV2.");

            if (createResultsRequestV2.ResultsOption.IsSet)
            {
                writer.WritePropertyName("results");
                JsonSerializer.Serialize(writer, createResultsRequestV2.Results, jsonSerializerOptions);
            }
        }
    }
}
