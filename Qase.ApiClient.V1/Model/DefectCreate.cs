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
    /// DefectCreate
    /// </summary>
    public partial class DefectCreate : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefectCreate" /> class.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="actualResult">actualResult</param>
        /// <param name="severity">severity</param>
        /// <param name="milestoneId">milestoneId</param>
        /// <param name="attachments">attachments</param>
        /// <param name="customField">A map of custom fields values (id &#x3D;&gt; value)</param>
        /// <param name="tags">tags</param>
        [JsonConstructor]
        public DefectCreate(string title, string actualResult, int severity, Option<long?> milestoneId = default, Option<List<string>?> attachments = default, Option<Dictionary<string, string>?> customField = default, Option<List<string>?> tags = default)
        {
            Title = title;
            ActualResult = actualResult;
            Severity = severity;
            MilestoneIdOption = milestoneId;
            AttachmentsOption = attachments;
            CustomFieldOption = customField;
            TagsOption = tags;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets ActualResult
        /// </summary>
        [JsonPropertyName("actual_result")]
        public string ActualResult { get; set; }

        /// <summary>
        /// Gets or Sets Severity
        /// </summary>
        [JsonPropertyName("severity")]
        public int Severity { get; set; }

        /// <summary>
        /// Used to track the state of MilestoneId
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<long?> MilestoneIdOption { get; private set; }

        /// <summary>
        /// Gets or Sets MilestoneId
        /// </summary>
        [JsonPropertyName("milestone_id")]
        public long? MilestoneId { get { return this.MilestoneIdOption; } set { this.MilestoneIdOption = new Option<long?>(value); } }

        /// <summary>
        /// Used to track the state of Attachments
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<List<string>?> AttachmentsOption { get; private set; }

        /// <summary>
        /// Gets or Sets Attachments
        /// </summary>
        [JsonPropertyName("attachments")]
        public List<string>? Attachments { get { return this.AttachmentsOption; } set { this.AttachmentsOption = new Option<List<string>?>(value); } }

        /// <summary>
        /// Used to track the state of CustomField
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<Dictionary<string, string>?> CustomFieldOption { get; private set; }

        /// <summary>
        /// A map of custom fields values (id &#x3D;&gt; value)
        /// </summary>
        /// <value>A map of custom fields values (id &#x3D;&gt; value)</value>
        [JsonPropertyName("custom_field")]
        public Dictionary<string, string>? CustomField { get { return this.CustomFieldOption; } set { this.CustomFieldOption = new Option<Dictionary<string, string>?>(value); } }

        /// <summary>
        /// Used to track the state of Tags
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<List<string>?> TagsOption { get; private set; }

        /// <summary>
        /// Gets or Sets Tags
        /// </summary>
        [JsonPropertyName("tags")]
        public List<string>? Tags { get { return this.TagsOption; } set { this.TagsOption = new Option<List<string>?>(value); } }

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
            sb.Append("class DefectCreate {\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  ActualResult: ").Append(ActualResult).Append("\n");
            sb.Append("  Severity: ").Append(Severity).Append("\n");
            sb.Append("  MilestoneId: ").Append(MilestoneId).Append("\n");
            sb.Append("  Attachments: ").Append(Attachments).Append("\n");
            sb.Append("  CustomField: ").Append(CustomField).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
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
    /// A Json converter for type <see cref="DefectCreate" />
    /// </summary>
    public class DefectCreateJsonConverter : JsonConverter<DefectCreate>
    {
        /// <summary>
        /// Deserializes json to <see cref="DefectCreate" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override DefectCreate Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            Option<string?> title = default;
            Option<string?> actualResult = default;
            Option<int?> severity = default;
            Option<long?> milestoneId = default;
            Option<List<string>?> attachments = default;
            Option<Dictionary<string, string>?> customField = default;
            Option<List<string>?> tags = default;

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
                        case "actual_result":
                            actualResult = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "severity":
                            severity = new Option<int?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (int?)null : utf8JsonReader.GetInt32());
                            break;
                        case "milestone_id":
                            milestoneId = new Option<long?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (long?)null : utf8JsonReader.GetInt64());
                            break;
                        case "attachments":
                            attachments = new Option<List<string>?>(JsonSerializer.Deserialize<List<string>>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        case "custom_field":
                            customField = new Option<Dictionary<string, string>?>(JsonSerializer.Deserialize<Dictionary<string, string>>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        case "tags":
                            tags = new Option<List<string>?>(JsonSerializer.Deserialize<List<string>>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!title.IsSet)
                throw new ArgumentException("Property is required for class DefectCreate.", nameof(title));

            if (!actualResult.IsSet)
                throw new ArgumentException("Property is required for class DefectCreate.", nameof(actualResult));

            if (!severity.IsSet)
                throw new ArgumentException("Property is required for class DefectCreate.", nameof(severity));

            if (title.IsSet && title.Value == null)
                throw new ArgumentNullException(nameof(title), "Property is not nullable for class DefectCreate.");

            if (actualResult.IsSet && actualResult.Value == null)
                throw new ArgumentNullException(nameof(actualResult), "Property is not nullable for class DefectCreate.");

            if (severity.IsSet && severity.Value == null)
                throw new ArgumentNullException(nameof(severity), "Property is not nullable for class DefectCreate.");

            if (attachments.IsSet && attachments.Value == null)
                throw new ArgumentNullException(nameof(attachments), "Property is not nullable for class DefectCreate.");

            if (customField.IsSet && customField.Value == null)
                throw new ArgumentNullException(nameof(customField), "Property is not nullable for class DefectCreate.");

            if (tags.IsSet && tags.Value == null)
                throw new ArgumentNullException(nameof(tags), "Property is not nullable for class DefectCreate.");

            return new DefectCreate(title.Value!, actualResult.Value!, severity.Value!.Value!, milestoneId, attachments, customField, tags);
        }

        /// <summary>
        /// Serializes a <see cref="DefectCreate" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="defectCreate"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, DefectCreate defectCreate, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            WriteProperties(writer, defectCreate, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="DefectCreate" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="defectCreate"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, DefectCreate defectCreate, JsonSerializerOptions jsonSerializerOptions)
        {
            if (defectCreate.Title == null)
                throw new ArgumentNullException(nameof(defectCreate.Title), "Property is required for class DefectCreate.");

            if (defectCreate.ActualResult == null)
                throw new ArgumentNullException(nameof(defectCreate.ActualResult), "Property is required for class DefectCreate.");

            if (defectCreate.AttachmentsOption.IsSet && defectCreate.Attachments == null)
                throw new ArgumentNullException(nameof(defectCreate.Attachments), "Property is required for class DefectCreate.");

            if (defectCreate.CustomFieldOption.IsSet && defectCreate.CustomField == null)
                throw new ArgumentNullException(nameof(defectCreate.CustomField), "Property is required for class DefectCreate.");

            if (defectCreate.TagsOption.IsSet && defectCreate.Tags == null)
                throw new ArgumentNullException(nameof(defectCreate.Tags), "Property is required for class DefectCreate.");

            writer.WriteString("title", defectCreate.Title);

            writer.WriteString("actual_result", defectCreate.ActualResult);

            writer.WriteNumber("severity", defectCreate.Severity);

            if (defectCreate.MilestoneIdOption.IsSet)
                if (defectCreate.MilestoneIdOption.Value != null)
                    writer.WriteNumber("milestone_id", defectCreate.MilestoneIdOption.Value!.Value);
                else
                    writer.WriteNull("milestone_id");

            if (defectCreate.AttachmentsOption.IsSet)
            {
                writer.WritePropertyName("attachments");
                JsonSerializer.Serialize(writer, defectCreate.Attachments, jsonSerializerOptions);
            }
            if (defectCreate.CustomFieldOption.IsSet)
            {
                writer.WritePropertyName("custom_field");
                JsonSerializer.Serialize(writer, defectCreate.CustomField, jsonSerializerOptions);
            }
            if (defectCreate.TagsOption.IsSet)
            {
                writer.WritePropertyName("tags");
                JsonSerializer.Serialize(writer, defectCreate.Tags, jsonSerializerOptions);
            }
        }
    }
}
