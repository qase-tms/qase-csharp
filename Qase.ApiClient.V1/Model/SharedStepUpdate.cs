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
    /// SharedStepUpdate
    /// </summary>
    public partial class SharedStepUpdate : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharedStepUpdate" /> class.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="action">Deprecated, use the &#x60;steps&#x60; property instead.</param>
        /// <param name="expectedResult">Deprecated, use the &#x60;steps&#x60; property instead.</param>
        /// <param name="data">Deprecated, use the &#x60;steps&#x60; property instead.</param>
        /// <param name="steps">steps</param>
        [JsonConstructor]
        public SharedStepUpdate(string title, Option<string?> action = default, Option<string?> expectedResult = default, Option<string?> data = default, Option<List<SharedStepContentCreate>?> steps = default)
        {
            Title = title;
            ActionOption = action;
            ExpectedResultOption = expectedResult;
            DataOption = data;
            StepsOption = steps;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Used to track the state of Action
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> ActionOption { get; private set; }

        /// <summary>
        /// Deprecated, use the &#x60;steps&#x60; property instead.
        /// </summary>
        /// <value>Deprecated, use the &#x60;steps&#x60; property instead.</value>
        [JsonPropertyName("action")]
        [Obsolete]
        public string? Action { get { return this.ActionOption; } set { this.ActionOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of ExpectedResult
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> ExpectedResultOption { get; private set; }

        /// <summary>
        /// Deprecated, use the &#x60;steps&#x60; property instead.
        /// </summary>
        /// <value>Deprecated, use the &#x60;steps&#x60; property instead.</value>
        [JsonPropertyName("expected_result")]
        [Obsolete]
        public string? ExpectedResult { get { return this.ExpectedResultOption; } set { this.ExpectedResultOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of Data
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> DataOption { get; private set; }

        /// <summary>
        /// Deprecated, use the &#x60;steps&#x60; property instead.
        /// </summary>
        /// <value>Deprecated, use the &#x60;steps&#x60; property instead.</value>
        [JsonPropertyName("data")]
        [Obsolete]
        public string? Data { get { return this.DataOption; } set { this.DataOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of Steps
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<List<SharedStepContentCreate>?> StepsOption { get; private set; }

        /// <summary>
        /// Gets or Sets Steps
        /// </summary>
        [JsonPropertyName("steps")]
        public List<SharedStepContentCreate>? Steps { get { return this.StepsOption; } set { this.StepsOption = new Option<List<SharedStepContentCreate>?>(value); } }

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
            sb.Append("class SharedStepUpdate {\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Action: ").Append(Action).Append("\n");
            sb.Append("  ExpectedResult: ").Append(ExpectedResult).Append("\n");
            sb.Append("  Data: ").Append(Data).Append("\n");
            sb.Append("  Steps: ").Append(Steps).Append("\n");
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
            // Title (string) maxLength
            if (this.Title != null && this.Title.Length > 255)
            {
                yield return new ValidationResult("Invalid value for Title, length must be less than 255.", new [] { "Title" });
            }

            yield break;
        }
    }

    /// <summary>
    /// A Json converter for type <see cref="SharedStepUpdate" />
    /// </summary>
    public class SharedStepUpdateJsonConverter : JsonConverter<SharedStepUpdate>
    {
        /// <summary>
        /// Deserializes json to <see cref="SharedStepUpdate" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override SharedStepUpdate Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            Option<string?> title = default;
            Option<string?> action = default;
            Option<string?> expectedResult = default;
            Option<string?> data = default;
            Option<List<SharedStepContentCreate>?> steps = default;

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
                        case "action":
                            action = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "expected_result":
                            expectedResult = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "data":
                            data = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "steps":
                            steps = new Option<List<SharedStepContentCreate>?>(JsonSerializer.Deserialize<List<SharedStepContentCreate>>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!title.IsSet)
                throw new ArgumentException("Property is required for class SharedStepUpdate.", nameof(title));

            if (title.IsSet && title.Value == null)
                throw new ArgumentNullException(nameof(title), "Property is not nullable for class SharedStepUpdate.");

            if (action.IsSet && action.Value == null)
                throw new ArgumentNullException(nameof(action), "Property is not nullable for class SharedStepUpdate.");

            if (expectedResult.IsSet && expectedResult.Value == null)
                throw new ArgumentNullException(nameof(expectedResult), "Property is not nullable for class SharedStepUpdate.");

            if (data.IsSet && data.Value == null)
                throw new ArgumentNullException(nameof(data), "Property is not nullable for class SharedStepUpdate.");

            if (steps.IsSet && steps.Value == null)
                throw new ArgumentNullException(nameof(steps), "Property is not nullable for class SharedStepUpdate.");

            return new SharedStepUpdate(title.Value!, action, expectedResult, data, steps);
        }

        /// <summary>
        /// Serializes a <see cref="SharedStepUpdate" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sharedStepUpdate"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, SharedStepUpdate sharedStepUpdate, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            WriteProperties(writer, sharedStepUpdate, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="SharedStepUpdate" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sharedStepUpdate"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, SharedStepUpdate sharedStepUpdate, JsonSerializerOptions jsonSerializerOptions)
        {
            if (sharedStepUpdate.Title == null)
                throw new ArgumentNullException(nameof(sharedStepUpdate.Title), "Property is required for class SharedStepUpdate.");

            if (sharedStepUpdate.ActionOption.IsSet && sharedStepUpdate.Action == null)
                throw new ArgumentNullException(nameof(sharedStepUpdate.Action), "Property is required for class SharedStepUpdate.");

            if (sharedStepUpdate.ExpectedResultOption.IsSet && sharedStepUpdate.ExpectedResult == null)
                throw new ArgumentNullException(nameof(sharedStepUpdate.ExpectedResult), "Property is required for class SharedStepUpdate.");

            if (sharedStepUpdate.DataOption.IsSet && sharedStepUpdate.Data == null)
                throw new ArgumentNullException(nameof(sharedStepUpdate.Data), "Property is required for class SharedStepUpdate.");

            if (sharedStepUpdate.StepsOption.IsSet && sharedStepUpdate.Steps == null)
                throw new ArgumentNullException(nameof(sharedStepUpdate.Steps), "Property is required for class SharedStepUpdate.");

            writer.WriteString("title", sharedStepUpdate.Title);

            if (sharedStepUpdate.ActionOption.IsSet)
                writer.WriteString("action", sharedStepUpdate.Action);

            if (sharedStepUpdate.ExpectedResultOption.IsSet)
                writer.WriteString("expected_result", sharedStepUpdate.ExpectedResult);

            if (sharedStepUpdate.DataOption.IsSet)
                writer.WriteString("data", sharedStepUpdate.Data);

            if (sharedStepUpdate.StepsOption.IsSet)
            {
                writer.WritePropertyName("steps");
                JsonSerializer.Serialize(writer, sharedStepUpdate.Steps, jsonSerializerOptions);
            }
        }
    }
}
