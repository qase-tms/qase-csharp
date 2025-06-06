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
    /// Result
    /// </summary>
    public partial class Result : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result" /> class.
        /// </summary>
        /// <param name="hash">hash</param>
        /// <param name="resultHash">resultHash</param>
        /// <param name="comment">comment</param>
        /// <param name="stacktrace">stacktrace</param>
        /// <param name="runId">runId</param>
        /// <param name="caseId">caseId</param>
        /// <param name="steps">steps</param>
        /// <param name="status">status</param>
        /// <param name="isApiResult">isApiResult</param>
        /// <param name="timeSpentMs">timeSpentMs</param>
        /// <param name="endTime">endTime</param>
        /// <param name="attachments">attachments</param>
        [JsonConstructor]
        public Result(Option<string?> hash = default, Option<string?> resultHash = default, Option<string?> comment = default, Option<string?> stacktrace = default, Option<long?> runId = default, Option<long?> caseId = default, Option<List<TestStepResult>?> steps = default, Option<string?> status = default, Option<bool?> isApiResult = default, Option<long?> timeSpentMs = default, Option<DateTime?> endTime = default, Option<List<Attachment>?> attachments = default)
        {
            HashOption = hash;
            ResultHashOption = resultHash;
            CommentOption = comment;
            StacktraceOption = stacktrace;
            RunIdOption = runId;
            CaseIdOption = caseId;
            StepsOption = steps;
            StatusOption = status;
            IsApiResultOption = isApiResult;
            TimeSpentMsOption = timeSpentMs;
            EndTimeOption = endTime;
            AttachmentsOption = attachments;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Used to track the state of Hash
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> HashOption { get; private set; }

        /// <summary>
        /// Gets or Sets Hash
        /// </summary>
        [JsonPropertyName("hash")]
        public string? Hash { get { return this.HashOption; } set { this.HashOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of ResultHash
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> ResultHashOption { get; private set; }

        /// <summary>
        /// Gets or Sets ResultHash
        /// </summary>
        [JsonPropertyName("result_hash")]
        public string? ResultHash { get { return this.ResultHashOption; } set { this.ResultHashOption = new Option<string?>(value); } }

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
        /// Used to track the state of Stacktrace
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> StacktraceOption { get; private set; }

        /// <summary>
        /// Gets or Sets Stacktrace
        /// </summary>
        [JsonPropertyName("stacktrace")]
        public string? Stacktrace { get { return this.StacktraceOption; } set { this.StacktraceOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of RunId
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<long?> RunIdOption { get; private set; }

        /// <summary>
        /// Gets or Sets RunId
        /// </summary>
        [JsonPropertyName("run_id")]
        public long? RunId { get { return this.RunIdOption; } set { this.RunIdOption = new Option<long?>(value); } }

        /// <summary>
        /// Used to track the state of CaseId
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<long?> CaseIdOption { get; private set; }

        /// <summary>
        /// Gets or Sets CaseId
        /// </summary>
        [JsonPropertyName("case_id")]
        public long? CaseId { get { return this.CaseIdOption; } set { this.CaseIdOption = new Option<long?>(value); } }

        /// <summary>
        /// Used to track the state of Steps
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<List<TestStepResult>?> StepsOption { get; private set; }

        /// <summary>
        /// Gets or Sets Steps
        /// </summary>
        [JsonPropertyName("steps")]
        public List<TestStepResult>? Steps { get { return this.StepsOption; } set { this.StepsOption = new Option<List<TestStepResult>?>(value); } }

        /// <summary>
        /// Used to track the state of Status
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> StatusOption { get; private set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get { return this.StatusOption; } set { this.StatusOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of IsApiResult
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<bool?> IsApiResultOption { get; private set; }

        /// <summary>
        /// Gets or Sets IsApiResult
        /// </summary>
        [JsonPropertyName("is_api_result")]
        public bool? IsApiResult { get { return this.IsApiResultOption; } set { this.IsApiResultOption = new Option<bool?>(value); } }

        /// <summary>
        /// Used to track the state of TimeSpentMs
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<long?> TimeSpentMsOption { get; private set; }

        /// <summary>
        /// Gets or Sets TimeSpentMs
        /// </summary>
        [JsonPropertyName("time_spent_ms")]
        public long? TimeSpentMs { get { return this.TimeSpentMsOption; } set { this.TimeSpentMsOption = new Option<long?>(value); } }

        /// <summary>
        /// Used to track the state of EndTime
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<DateTime?> EndTimeOption { get; private set; }

        /// <summary>
        /// Gets or Sets EndTime
        /// </summary>
        /* <example>2021-12-30T19:23:59Z</example> */
        [JsonPropertyName("end_time")]
        public DateTime? EndTime { get { return this.EndTimeOption; } set { this.EndTimeOption = new Option<DateTime?>(value); } }

        /// <summary>
        /// Used to track the state of Attachments
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<List<Attachment>?> AttachmentsOption { get; private set; }

        /// <summary>
        /// Gets or Sets Attachments
        /// </summary>
        [JsonPropertyName("attachments")]
        public List<Attachment>? Attachments { get { return this.AttachmentsOption; } set { this.AttachmentsOption = new Option<List<Attachment>?>(value); } }

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
            sb.Append("class Result {\n");
            sb.Append("  Hash: ").Append(Hash).Append("\n");
            sb.Append("  ResultHash: ").Append(ResultHash).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  Stacktrace: ").Append(Stacktrace).Append("\n");
            sb.Append("  RunId: ").Append(RunId).Append("\n");
            sb.Append("  CaseId: ").Append(CaseId).Append("\n");
            sb.Append("  Steps: ").Append(Steps).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  IsApiResult: ").Append(IsApiResult).Append("\n");
            sb.Append("  TimeSpentMs: ").Append(TimeSpentMs).Append("\n");
            sb.Append("  EndTime: ").Append(EndTime).Append("\n");
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
    /// A Json converter for type <see cref="Result" />
    /// </summary>
    public class ResultJsonConverter : JsonConverter<Result>
    {
        /// <summary>
        /// The format to use to serialize EndTime
        /// </summary>
        public static string EndTimeFormat { get; set; } = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK";

        /// <summary>
        /// Deserializes json to <see cref="Result" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override Result Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            Option<string?> hash = default;
            Option<string?> resultHash = default;
            Option<string?> comment = default;
            Option<string?> stacktrace = default;
            Option<long?> runId = default;
            Option<long?> caseId = default;
            Option<List<TestStepResult>?> steps = default;
            Option<string?> status = default;
            Option<bool?> isApiResult = default;
            Option<long?> timeSpentMs = default;
            Option<DateTime?> endTime = default;
            Option<List<Attachment>?> attachments = default;

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
                        case "hash":
                            hash = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "result_hash":
                            resultHash = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "comment":
                            comment = new Option<string?>(utf8JsonReader.GetString());
                            break;
                        case "stacktrace":
                            stacktrace = new Option<string?>(utf8JsonReader.GetString());
                            break;
                        case "run_id":
                            runId = new Option<long?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (long?)null : utf8JsonReader.GetInt64());
                            break;
                        case "case_id":
                            caseId = new Option<long?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (long?)null : utf8JsonReader.GetInt64());
                            break;
                        case "steps":
                            steps = new Option<List<TestStepResult>?>(JsonSerializer.Deserialize<List<TestStepResult>>(ref utf8JsonReader, jsonSerializerOptions));
                            break;
                        case "status":
                            status = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "is_api_result":
                            isApiResult = new Option<bool?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (bool?)null : utf8JsonReader.GetBoolean());
                            break;
                        case "time_spent_ms":
                            timeSpentMs = new Option<long?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (long?)null : utf8JsonReader.GetInt64());
                            break;
                        case "end_time":
                            endTime = new Option<DateTime?>(JsonSerializer.Deserialize<DateTime?>(ref utf8JsonReader, jsonSerializerOptions));
                            break;
                        case "attachments":
                            attachments = new Option<List<Attachment>?>(JsonSerializer.Deserialize<List<Attachment>>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (hash.IsSet && hash.Value == null)
                throw new ArgumentNullException(nameof(hash), "Property is not nullable for class Result.");

            if (resultHash.IsSet && resultHash.Value == null)
                throw new ArgumentNullException(nameof(resultHash), "Property is not nullable for class Result.");

            if (runId.IsSet && runId.Value == null)
                throw new ArgumentNullException(nameof(runId), "Property is not nullable for class Result.");

            if (caseId.IsSet && caseId.Value == null)
                throw new ArgumentNullException(nameof(caseId), "Property is not nullable for class Result.");

            if (status.IsSet && status.Value == null)
                throw new ArgumentNullException(nameof(status), "Property is not nullable for class Result.");

            if (isApiResult.IsSet && isApiResult.Value == null)
                throw new ArgumentNullException(nameof(isApiResult), "Property is not nullable for class Result.");

            if (timeSpentMs.IsSet && timeSpentMs.Value == null)
                throw new ArgumentNullException(nameof(timeSpentMs), "Property is not nullable for class Result.");

            if (attachments.IsSet && attachments.Value == null)
                throw new ArgumentNullException(nameof(attachments), "Property is not nullable for class Result.");

            return new Result(hash, resultHash, comment, stacktrace, runId, caseId, steps, status, isApiResult, timeSpentMs, endTime, attachments);
        }

        /// <summary>
        /// Serializes a <see cref="Result" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="result"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, Result result, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            WriteProperties(writer, result, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="Result" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="result"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, Result result, JsonSerializerOptions jsonSerializerOptions)
        {
            if (result.HashOption.IsSet && result.Hash == null)
                throw new ArgumentNullException(nameof(result.Hash), "Property is required for class Result.");

            if (result.ResultHashOption.IsSet && result.ResultHash == null)
                throw new ArgumentNullException(nameof(result.ResultHash), "Property is required for class Result.");

            if (result.StatusOption.IsSet && result.Status == null)
                throw new ArgumentNullException(nameof(result.Status), "Property is required for class Result.");

            if (result.AttachmentsOption.IsSet && result.Attachments == null)
                throw new ArgumentNullException(nameof(result.Attachments), "Property is required for class Result.");

            if (result.HashOption.IsSet)
                writer.WriteString("hash", result.Hash);

            if (result.ResultHashOption.IsSet)
                writer.WriteString("result_hash", result.ResultHash);

            if (result.CommentOption.IsSet)
                if (result.CommentOption.Value != null)
                    writer.WriteString("comment", result.Comment);
                else
                    writer.WriteNull("comment");

            if (result.StacktraceOption.IsSet)
                if (result.StacktraceOption.Value != null)
                    writer.WriteString("stacktrace", result.Stacktrace);
                else
                    writer.WriteNull("stacktrace");

            if (result.RunIdOption.IsSet)
                writer.WriteNumber("run_id", result.RunIdOption.Value!.Value);

            if (result.CaseIdOption.IsSet)
                writer.WriteNumber("case_id", result.CaseIdOption.Value!.Value);

            if (result.StepsOption.IsSet)
                if (result.StepsOption.Value != null)
                {
                    writer.WritePropertyName("steps");
                    JsonSerializer.Serialize(writer, result.Steps, jsonSerializerOptions);
                }
                else
                    writer.WriteNull("steps");
            if (result.StatusOption.IsSet)
                writer.WriteString("status", result.Status);

            if (result.IsApiResultOption.IsSet)
                writer.WriteBoolean("is_api_result", result.IsApiResultOption.Value!.Value);

            if (result.TimeSpentMsOption.IsSet)
                writer.WriteNumber("time_spent_ms", result.TimeSpentMsOption.Value!.Value);

            if (result.EndTimeOption.IsSet)
                if (result.EndTimeOption.Value != null)
                    writer.WriteString("end_time", result.EndTimeOption.Value!.Value.ToString(EndTimeFormat));
                else
                    writer.WriteNull("end_time");

            if (result.AttachmentsOption.IsSet)
            {
                writer.WritePropertyName("attachments");
                JsonSerializer.Serialize(writer, result.Attachments, jsonSerializerOptions);
            }
        }
    }
}
