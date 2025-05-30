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
    /// SuiteUpdate
    /// </summary>
    public partial class SuiteUpdate : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteUpdate" /> class.
        /// </summary>
        /// <param name="title">Test suite title.</param>
        /// <param name="description">Test suite description.</param>
        /// <param name="preconditions">Test suite preconditions</param>
        /// <param name="parentId">Parent suite ID</param>
        [JsonConstructor]
        public SuiteUpdate(Option<string?> title = default, Option<string?> description = default, Option<string?> preconditions = default, Option<long?> parentId = default)
        {
            TitleOption = title;
            DescriptionOption = description;
            PreconditionsOption = preconditions;
            ParentIdOption = parentId;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Used to track the state of Title
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> TitleOption { get; private set; }

        /// <summary>
        /// Test suite title.
        /// </summary>
        /// <value>Test suite title.</value>
        [JsonPropertyName("title")]
        public string? Title { get { return this.TitleOption; } set { this.TitleOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of Description
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> DescriptionOption { get; private set; }

        /// <summary>
        /// Test suite description.
        /// </summary>
        /// <value>Test suite description.</value>
        [JsonPropertyName("description")]
        public string? Description { get { return this.DescriptionOption; } set { this.DescriptionOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of Preconditions
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> PreconditionsOption { get; private set; }

        /// <summary>
        /// Test suite preconditions
        /// </summary>
        /// <value>Test suite preconditions</value>
        [JsonPropertyName("preconditions")]
        public string? Preconditions { get { return this.PreconditionsOption; } set { this.PreconditionsOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of ParentId
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<long?> ParentIdOption { get; private set; }

        /// <summary>
        /// Parent suite ID
        /// </summary>
        /// <value>Parent suite ID</value>
        [JsonPropertyName("parent_id")]
        public long? ParentId { get { return this.ParentIdOption; } set { this.ParentIdOption = new Option<long?>(value); } }

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
            sb.Append("class SuiteUpdate {\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Preconditions: ").Append(Preconditions).Append("\n");
            sb.Append("  ParentId: ").Append(ParentId).Append("\n");
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
    /// A Json converter for type <see cref="SuiteUpdate" />
    /// </summary>
    public class SuiteUpdateJsonConverter : JsonConverter<SuiteUpdate>
    {
        /// <summary>
        /// Deserializes json to <see cref="SuiteUpdate" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override SuiteUpdate Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            Option<string?> title = default;
            Option<string?> description = default;
            Option<string?> preconditions = default;
            Option<long?> parentId = default;

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
                        case "title":
                            title = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "description":
                            description = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "preconditions":
                            preconditions = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "parent_id":
                            parentId = new Option<long?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (long?)null : utf8JsonReader.GetInt64());
                            break;
                        default:
                            break;
                    }
                }
            }

            if (title.IsSet && title.Value == null)
                throw new ArgumentNullException(nameof(title), "Property is not nullable for class SuiteUpdate.");

            if (description.IsSet && description.Value == null)
                throw new ArgumentNullException(nameof(description), "Property is not nullable for class SuiteUpdate.");

            if (preconditions.IsSet && preconditions.Value == null)
                throw new ArgumentNullException(nameof(preconditions), "Property is not nullable for class SuiteUpdate.");

            return new SuiteUpdate(title, description, preconditions, parentId);
        }

        /// <summary>
        /// Serializes a <see cref="SuiteUpdate" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="suiteUpdate"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, SuiteUpdate suiteUpdate, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            WriteProperties(writer, suiteUpdate, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="SuiteUpdate" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="suiteUpdate"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, SuiteUpdate suiteUpdate, JsonSerializerOptions jsonSerializerOptions)
        {
            if (suiteUpdate.TitleOption.IsSet && suiteUpdate.Title == null)
                throw new ArgumentNullException(nameof(suiteUpdate.Title), "Property is required for class SuiteUpdate.");

            if (suiteUpdate.DescriptionOption.IsSet && suiteUpdate.Description == null)
                throw new ArgumentNullException(nameof(suiteUpdate.Description), "Property is required for class SuiteUpdate.");

            if (suiteUpdate.PreconditionsOption.IsSet && suiteUpdate.Preconditions == null)
                throw new ArgumentNullException(nameof(suiteUpdate.Preconditions), "Property is required for class SuiteUpdate.");

            if (suiteUpdate.TitleOption.IsSet)
                writer.WriteString("title", suiteUpdate.Title);

            if (suiteUpdate.DescriptionOption.IsSet)
                writer.WriteString("description", suiteUpdate.Description);

            if (suiteUpdate.PreconditionsOption.IsSet)
                writer.WriteString("preconditions", suiteUpdate.Preconditions);

            if (suiteUpdate.ParentIdOption.IsSet)
                if (suiteUpdate.ParentIdOption.Value != null)
                    writer.WriteNumber("parent_id", suiteUpdate.ParentIdOption.Value!.Value);
                else
                    writer.WriteNull("parent_id");
        }
    }
}
