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
    /// ResultStepExecution
    /// </summary>
    public partial class ResultStepExecution : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultStepExecution" /> class.
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="startTime">Unix epoch time in seconds (whole part) and milliseconds (fractional part).</param>
        /// <param name="endTime">Unix epoch time in seconds (whole part) and milliseconds (fractional part).</param>
        /// <param name="duration">Duration of the test step execution in milliseconds.</param>
        /// <param name="comment">comment</param>
        /// <param name="attachments">attachments</param>
        [JsonConstructor]
        public ResultStepExecution(ResultStepStatus status, Option<double?> startTime = default, Option<double?> endTime = default, Option<long?> duration = default, Option<string?> comment = default, Option<List<string>?> attachments = default)
        {
            Status = status;
            StartTimeOption = startTime;
            EndTimeOption = endTime;
            DurationOption = duration;
            CommentOption = comment;
            AttachmentsOption = attachments;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [JsonPropertyName("status")]
        public ResultStepStatus Status { get; set; }

        /// <summary>
        /// Used to track the state of StartTime
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<double?> StartTimeOption { get; private set; }

        /// <summary>
        /// Unix epoch time in seconds (whole part) and milliseconds (fractional part).
        /// </summary>
        /// <value>Unix epoch time in seconds (whole part) and milliseconds (fractional part).</value>
        [JsonPropertyName("start_time")]
        public double? StartTime { get { return this.StartTimeOption; } set { this.StartTimeOption = new Option<double?>(value); } }

        /// <summary>
        /// Used to track the state of EndTime
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<double?> EndTimeOption { get; private set; }

        /// <summary>
        /// Unix epoch time in seconds (whole part) and milliseconds (fractional part).
        /// </summary>
        /// <value>Unix epoch time in seconds (whole part) and milliseconds (fractional part).</value>
        [JsonPropertyName("end_time")]
        public double? EndTime { get { return this.EndTimeOption; } set { this.EndTimeOption = new Option<double?>(value); } }

        /// <summary>
        /// Used to track the state of Duration
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<long?> DurationOption { get; private set; }

        /// <summary>
        /// Duration of the test step execution in milliseconds.
        /// </summary>
        /// <value>Duration of the test step execution in milliseconds.</value>
        [JsonPropertyName("duration")]
        public long? Duration { get { return this.DurationOption; } set { this.DurationOption = new Option<long?>(value); } }

        /// <summary>
        /// Used to track the state of Comment
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> CommentOption { get; private set; }

        /// <summary>
        /// Gets or Sets Comment
        /// </summary>
        [JsonPropertyName("comment")]
        public string? Comment { get { return this.CommentOption; } set { this.CommentOption = new Option<string?>(value); } }

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
            sb.Append("class ResultStepExecution {\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  StartTime: ").Append(StartTime).Append("\n");
            sb.Append("  EndTime: ").Append(EndTime).Append("\n");
            sb.Append("  Duration: ").Append(Duration).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  Attachments: ").Append(Attachments).Append("\n");
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
    /// A Json converter for type <see cref="ResultStepExecution" />
    /// </summary>
    public class ResultStepExecutionJsonConverter : JsonConverter<ResultStepExecution>
    {
        /// <summary>
        /// Deserializes json to <see cref="ResultStepExecution" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override ResultStepExecution Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            Option<ResultStepStatus?> status = default;
            Option<double?> startTime = default;
            Option<double?> endTime = default;
            Option<long?> duration = default;
            Option<string?> comment = default;
            Option<List<string>?> attachments = default;

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
                        case "status":
                            string? statusRawValue = utf8JsonReader.GetString();
                            if (statusRawValue != null)
                                status = new Option<ResultStepStatus?>(ResultStepStatusValueConverter.FromStringOrDefault(statusRawValue));
                            break;
                        case "start_time":
                            startTime = new Option<double?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (double?)null : utf8JsonReader.GetDouble());
                            break;
                        case "end_time":
                            endTime = new Option<double?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (double?)null : utf8JsonReader.GetDouble());
                            break;
                        case "duration":
                            duration = new Option<long?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (long?)null : utf8JsonReader.GetInt64());
                            break;
                        case "comment":
                            comment = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "attachments":
                            attachments = new Option<List<string>?>(JsonSerializer.Deserialize<List<string>>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!status.IsSet)
                throw new ArgumentException("Property is required for class ResultStepExecution.", nameof(status));

            if (status.IsSet && status.Value == null)
                throw new ArgumentNullException(nameof(status), "Property is not nullable for class ResultStepExecution.");

            if (comment.IsSet && comment.Value == null)
                throw new ArgumentNullException(nameof(comment), "Property is not nullable for class ResultStepExecution.");

            if (attachments.IsSet && attachments.Value == null)
                throw new ArgumentNullException(nameof(attachments), "Property is not nullable for class ResultStepExecution.");

            return new ResultStepExecution(status.Value!.Value!, startTime, endTime, duration, comment, attachments);
        }

        /// <summary>
        /// Serializes a <see cref="ResultStepExecution" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="resultStepExecution"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, ResultStepExecution resultStepExecution, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            WriteProperties(writer, resultStepExecution, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="ResultStepExecution" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="resultStepExecution"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, ResultStepExecution resultStepExecution, JsonSerializerOptions jsonSerializerOptions)
        {
            if (resultStepExecution.CommentOption.IsSet && resultStepExecution.Comment == null)
                throw new ArgumentNullException(nameof(resultStepExecution.Comment), "Property is required for class ResultStepExecution.");

            if (resultStepExecution.AttachmentsOption.IsSet && resultStepExecution.Attachments == null)
                throw new ArgumentNullException(nameof(resultStepExecution.Attachments), "Property is required for class ResultStepExecution.");

            var statusRawValue = ResultStepStatusValueConverter.ToJsonValue(resultStepExecution.Status);
            writer.WriteString("status", statusRawValue);

            if (resultStepExecution.StartTimeOption.IsSet)
                if (resultStepExecution.StartTimeOption.Value != null)
                    writer.WriteNumber("start_time", resultStepExecution.StartTimeOption.Value!.Value);
                else
                    writer.WriteNull("start_time");

            if (resultStepExecution.EndTimeOption.IsSet)
                if (resultStepExecution.EndTimeOption.Value != null)
                    writer.WriteNumber("end_time", resultStepExecution.EndTimeOption.Value!.Value);
                else
                    writer.WriteNull("end_time");

            if (resultStepExecution.DurationOption.IsSet)
                if (resultStepExecution.DurationOption.Value != null)
                    writer.WriteNumber("duration", resultStepExecution.DurationOption.Value!.Value);
                else
                    writer.WriteNull("duration");

            if (resultStepExecution.CommentOption.IsSet)
                writer.WriteString("comment", resultStepExecution.Comment);

            if (resultStepExecution.AttachmentsOption.IsSet)
            {
                writer.WritePropertyName("attachments");
                JsonSerializer.Serialize(writer, resultStepExecution.Attachments, jsonSerializerOptions);
            }
        }
    }
}
