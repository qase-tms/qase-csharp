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
    /// RunCreate
    /// </summary>
    public partial class RunCreate : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunCreate" /> class.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="description">description</param>
        /// <param name="includeAllCases">includeAllCases</param>
        /// <param name="cases">cases</param>
        /// <param name="isAutotest">isAutotest</param>
        /// <param name="environmentId">environmentId</param>
        /// <param name="environmentSlug">environmentSlug</param>
        /// <param name="milestoneId">milestoneId</param>
        /// <param name="planId">planId</param>
        /// <param name="authorId">authorId</param>
        /// <param name="tags">tags</param>
        /// <param name="configurations">configurations</param>
        /// <param name="customField">A map of custom fields values (id &#x3D;&gt; value)</param>
        /// <param name="startTime">startTime</param>
        /// <param name="endTime">endTime</param>
        [JsonConstructor]
        public RunCreate(string title, Option<string?> description = default, Option<bool?> includeAllCases = default, Option<List<long>?> cases = default, Option<bool?> isAutotest = default, Option<long?> environmentId = default, Option<string?> environmentSlug = default, Option<long?> milestoneId = default, Option<long?> planId = default, Option<long?> authorId = default, Option<List<string>?> tags = default, Option<List<long>?> configurations = default, Option<Dictionary<string, string>?> customField = default, Option<string?> startTime = default, Option<string?> endTime = default)
        {
            Title = title;
            DescriptionOption = description;
            IncludeAllCasesOption = includeAllCases;
            CasesOption = cases;
            IsAutotestOption = isAutotest;
            EnvironmentIdOption = environmentId;
            EnvironmentSlugOption = environmentSlug;
            MilestoneIdOption = milestoneId;
            PlanIdOption = planId;
            AuthorIdOption = authorId;
            TagsOption = tags;
            ConfigurationsOption = configurations;
            CustomFieldOption = customField;
            StartTimeOption = startTime;
            EndTimeOption = endTime;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Used to track the state of Description
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> DescriptionOption { get; private set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get { return this.DescriptionOption; } set { this.DescriptionOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of IncludeAllCases
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<bool?> IncludeAllCasesOption { get; private set; }

        /// <summary>
        /// Gets or Sets IncludeAllCases
        /// </summary>
        [JsonPropertyName("include_all_cases")]
        public bool? IncludeAllCases { get { return this.IncludeAllCasesOption; } set { this.IncludeAllCasesOption = new Option<bool?>(value); } }

        /// <summary>
        /// Used to track the state of Cases
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<List<long>?> CasesOption { get; private set; }

        /// <summary>
        /// Gets or Sets Cases
        /// </summary>
        [JsonPropertyName("cases")]
        public List<long>? Cases { get { return this.CasesOption; } set { this.CasesOption = new Option<List<long>?>(value); } }

        /// <summary>
        /// Used to track the state of IsAutotest
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<bool?> IsAutotestOption { get; private set; }

        /// <summary>
        /// Gets or Sets IsAutotest
        /// </summary>
        [JsonPropertyName("is_autotest")]
        public bool? IsAutotest { get { return this.IsAutotestOption; } set { this.IsAutotestOption = new Option<bool?>(value); } }

        /// <summary>
        /// Used to track the state of EnvironmentId
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<long?> EnvironmentIdOption { get; private set; }

        /// <summary>
        /// Gets or Sets EnvironmentId
        /// </summary>
        [JsonPropertyName("environment_id")]
        public long? EnvironmentId { get { return this.EnvironmentIdOption; } set { this.EnvironmentIdOption = new Option<long?>(value); } }

        /// <summary>
        /// Used to track the state of EnvironmentSlug
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> EnvironmentSlugOption { get; private set; }

        /// <summary>
        /// Gets or Sets EnvironmentSlug
        /// </summary>
        [JsonPropertyName("environment_slug")]
        public string? EnvironmentSlug { get { return this.EnvironmentSlugOption; } set { this.EnvironmentSlugOption = new Option<string?>(value); } }

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
        /// Used to track the state of PlanId
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<long?> PlanIdOption { get; private set; }

        /// <summary>
        /// Gets or Sets PlanId
        /// </summary>
        [JsonPropertyName("plan_id")]
        public long? PlanId { get { return this.PlanIdOption; } set { this.PlanIdOption = new Option<long?>(value); } }

        /// <summary>
        /// Used to track the state of AuthorId
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<long?> AuthorIdOption { get; private set; }

        /// <summary>
        /// Gets or Sets AuthorId
        /// </summary>
        [JsonPropertyName("author_id")]
        public long? AuthorId { get { return this.AuthorIdOption; } set { this.AuthorIdOption = new Option<long?>(value); } }

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
        /// Used to track the state of Configurations
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<List<long>?> ConfigurationsOption { get; private set; }

        /// <summary>
        /// Gets or Sets Configurations
        /// </summary>
        [JsonPropertyName("configurations")]
        public List<long>? Configurations { get { return this.ConfigurationsOption; } set { this.ConfigurationsOption = new Option<List<long>?>(value); } }

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
        /// Used to track the state of StartTime
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> StartTimeOption { get; private set; }

        /// <summary>
        /// Gets or Sets StartTime
        /// </summary>
        [JsonPropertyName("start_time")]
        public string? StartTime { get { return this.StartTimeOption; } set { this.StartTimeOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of EndTime
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> EndTimeOption { get; private set; }

        /// <summary>
        /// Gets or Sets EndTime
        /// </summary>
        [JsonPropertyName("end_time")]
        public string? EndTime { get { return this.EndTimeOption; } set { this.EndTimeOption = new Option<string?>(value); } }

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
            sb.Append("class RunCreate {\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  IncludeAllCases: ").Append(IncludeAllCases).Append("\n");
            sb.Append("  Cases: ").Append(Cases).Append("\n");
            sb.Append("  IsAutotest: ").Append(IsAutotest).Append("\n");
            sb.Append("  EnvironmentId: ").Append(EnvironmentId).Append("\n");
            sb.Append("  EnvironmentSlug: ").Append(EnvironmentSlug).Append("\n");
            sb.Append("  MilestoneId: ").Append(MilestoneId).Append("\n");
            sb.Append("  PlanId: ").Append(PlanId).Append("\n");
            sb.Append("  AuthorId: ").Append(AuthorId).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
            sb.Append("  Configurations: ").Append(Configurations).Append("\n");
            sb.Append("  CustomField: ").Append(CustomField).Append("\n");
            sb.Append("  StartTime: ").Append(StartTime).Append("\n");
            sb.Append("  EndTime: ").Append(EndTime).Append("\n");
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

            // Description (string) maxLength
            if (this.Description != null && this.Description.Length > 10000)
            {
                yield return new ValidationResult("Invalid value for Description, length must be less than 10000.", new [] { "Description" });
            }

            // EnvironmentId (long) minimum
            if (this.EnvironmentIdOption.IsSet && this.EnvironmentIdOption.Value < (long)1)
            {
                yield return new ValidationResult("Invalid value for EnvironmentId, must be a value greater than or equal to 1.", new [] { "EnvironmentId" });
            }

            // EnvironmentSlug (string) maxLength
            if (this.EnvironmentSlug != null && this.EnvironmentSlug.Length > 255)
            {
                yield return new ValidationResult("Invalid value for EnvironmentSlug, length must be less than 255.", new [] { "EnvironmentSlug" });
            }

            // MilestoneId (long) minimum
            if (this.MilestoneIdOption.IsSet && this.MilestoneIdOption.Value < (long)1)
            {
                yield return new ValidationResult("Invalid value for MilestoneId, must be a value greater than or equal to 1.", new [] { "MilestoneId" });
            }

            // PlanId (long) minimum
            if (this.PlanIdOption.IsSet && this.PlanIdOption.Value < (long)1)
            {
                yield return new ValidationResult("Invalid value for PlanId, must be a value greater than or equal to 1.", new [] { "PlanId" });
            }

            // AuthorId (long) minimum
            if (this.AuthorIdOption.IsSet && this.AuthorIdOption.Value < (long)1)
            {
                yield return new ValidationResult("Invalid value for AuthorId, must be a value greater than or equal to 1.", new [] { "AuthorId" });
            }

            yield break;
        }
    }

    /// <summary>
    /// A Json converter for type <see cref="RunCreate" />
    /// </summary>
    public class RunCreateJsonConverter : JsonConverter<RunCreate>
    {
        /// <summary>
        /// Deserializes json to <see cref="RunCreate" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override RunCreate Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            Option<string?> title = default;
            Option<string?> description = default;
            Option<bool?> includeAllCases = default;
            Option<List<long>?> cases = default;
            Option<bool?> isAutotest = default;
            Option<long?> environmentId = default;
            Option<string?> environmentSlug = default;
            Option<long?> milestoneId = default;
            Option<long?> planId = default;
            Option<long?> authorId = default;
            Option<List<string>?> tags = default;
            Option<List<long>?> configurations = default;
            Option<Dictionary<string, string>?> customField = default;
            Option<string?> startTime = default;
            Option<string?> endTime = default;

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
                        case "include_all_cases":
                            includeAllCases = new Option<bool?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (bool?)null : utf8JsonReader.GetBoolean());
                            break;
                        case "cases":
                            cases = new Option<List<long>?>(JsonSerializer.Deserialize<List<long>>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        case "is_autotest":
                            isAutotest = new Option<bool?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (bool?)null : utf8JsonReader.GetBoolean());
                            break;
                        case "environment_id":
                            environmentId = new Option<long?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (long?)null : utf8JsonReader.GetInt64());
                            break;
                        case "environment_slug":
                            environmentSlug = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "milestone_id":
                            milestoneId = new Option<long?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (long?)null : utf8JsonReader.GetInt64());
                            break;
                        case "plan_id":
                            planId = new Option<long?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (long?)null : utf8JsonReader.GetInt64());
                            break;
                        case "author_id":
                            authorId = new Option<long?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (long?)null : utf8JsonReader.GetInt64());
                            break;
                        case "tags":
                            tags = new Option<List<string>?>(JsonSerializer.Deserialize<List<string>>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        case "configurations":
                            configurations = new Option<List<long>?>(JsonSerializer.Deserialize<List<long>>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        case "custom_field":
                            customField = new Option<Dictionary<string, string>?>(JsonSerializer.Deserialize<Dictionary<string, string>>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        case "start_time":
                            startTime = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "end_time":
                            endTime = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!title.IsSet)
                throw new ArgumentException("Property is required for class RunCreate.", nameof(title));

            if (title.IsSet && title.Value == null)
                throw new ArgumentNullException(nameof(title), "Property is not nullable for class RunCreate.");

            if (description.IsSet && description.Value == null)
                throw new ArgumentNullException(nameof(description), "Property is not nullable for class RunCreate.");

            if (includeAllCases.IsSet && includeAllCases.Value == null)
                throw new ArgumentNullException(nameof(includeAllCases), "Property is not nullable for class RunCreate.");

            if (cases.IsSet && cases.Value == null)
                throw new ArgumentNullException(nameof(cases), "Property is not nullable for class RunCreate.");

            if (isAutotest.IsSet && isAutotest.Value == null)
                throw new ArgumentNullException(nameof(isAutotest), "Property is not nullable for class RunCreate.");

            if (environmentId.IsSet && environmentId.Value == null)
                throw new ArgumentNullException(nameof(environmentId), "Property is not nullable for class RunCreate.");

            if (environmentSlug.IsSet && environmentSlug.Value == null)
                throw new ArgumentNullException(nameof(environmentSlug), "Property is not nullable for class RunCreate.");

            if (milestoneId.IsSet && milestoneId.Value == null)
                throw new ArgumentNullException(nameof(milestoneId), "Property is not nullable for class RunCreate.");

            if (planId.IsSet && planId.Value == null)
                throw new ArgumentNullException(nameof(planId), "Property is not nullable for class RunCreate.");

            if (authorId.IsSet && authorId.Value == null)
                throw new ArgumentNullException(nameof(authorId), "Property is not nullable for class RunCreate.");

            if (tags.IsSet && tags.Value == null)
                throw new ArgumentNullException(nameof(tags), "Property is not nullable for class RunCreate.");

            if (configurations.IsSet && configurations.Value == null)
                throw new ArgumentNullException(nameof(configurations), "Property is not nullable for class RunCreate.");

            if (customField.IsSet && customField.Value == null)
                throw new ArgumentNullException(nameof(customField), "Property is not nullable for class RunCreate.");

            if (startTime.IsSet && startTime.Value == null)
                throw new ArgumentNullException(nameof(startTime), "Property is not nullable for class RunCreate.");

            if (endTime.IsSet && endTime.Value == null)
                throw new ArgumentNullException(nameof(endTime), "Property is not nullable for class RunCreate.");

            return new RunCreate(title.Value!, description, includeAllCases, cases, isAutotest, environmentId, environmentSlug, milestoneId, planId, authorId, tags, configurations, customField, startTime, endTime);
        }

        /// <summary>
        /// Serializes a <see cref="RunCreate" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="runCreate"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, RunCreate runCreate, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            WriteProperties(writer, runCreate, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="RunCreate" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="runCreate"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, RunCreate runCreate, JsonSerializerOptions jsonSerializerOptions)
        {
            if (runCreate.Title == null)
                throw new ArgumentNullException(nameof(runCreate.Title), "Property is required for class RunCreate.");

            if (runCreate.DescriptionOption.IsSet && runCreate.Description == null)
                throw new ArgumentNullException(nameof(runCreate.Description), "Property is required for class RunCreate.");

            if (runCreate.CasesOption.IsSet && runCreate.Cases == null)
                throw new ArgumentNullException(nameof(runCreate.Cases), "Property is required for class RunCreate.");

            if (runCreate.EnvironmentSlugOption.IsSet && runCreate.EnvironmentSlug == null)
                throw new ArgumentNullException(nameof(runCreate.EnvironmentSlug), "Property is required for class RunCreate.");

            if (runCreate.TagsOption.IsSet && runCreate.Tags == null)
                throw new ArgumentNullException(nameof(runCreate.Tags), "Property is required for class RunCreate.");

            if (runCreate.ConfigurationsOption.IsSet && runCreate.Configurations == null)
                throw new ArgumentNullException(nameof(runCreate.Configurations), "Property is required for class RunCreate.");

            if (runCreate.CustomFieldOption.IsSet && runCreate.CustomField == null)
                throw new ArgumentNullException(nameof(runCreate.CustomField), "Property is required for class RunCreate.");

            if (runCreate.StartTimeOption.IsSet && runCreate.StartTime == null)
                throw new ArgumentNullException(nameof(runCreate.StartTime), "Property is required for class RunCreate.");

            if (runCreate.EndTimeOption.IsSet && runCreate.EndTime == null)
                throw new ArgumentNullException(nameof(runCreate.EndTime), "Property is required for class RunCreate.");

            writer.WriteString("title", runCreate.Title);

            if (runCreate.DescriptionOption.IsSet)
                writer.WriteString("description", runCreate.Description);

            if (runCreate.IncludeAllCasesOption.IsSet)
                writer.WriteBoolean("include_all_cases", runCreate.IncludeAllCasesOption.Value!.Value);

            if (runCreate.CasesOption.IsSet)
            {
                writer.WritePropertyName("cases");
                JsonSerializer.Serialize(writer, runCreate.Cases, jsonSerializerOptions);
            }
            if (runCreate.IsAutotestOption.IsSet)
                writer.WriteBoolean("is_autotest", runCreate.IsAutotestOption.Value!.Value);

            if (runCreate.EnvironmentIdOption.IsSet)
                writer.WriteNumber("environment_id", runCreate.EnvironmentIdOption.Value!.Value);

            if (runCreate.EnvironmentSlugOption.IsSet)
                writer.WriteString("environment_slug", runCreate.EnvironmentSlug);

            if (runCreate.MilestoneIdOption.IsSet)
                writer.WriteNumber("milestone_id", runCreate.MilestoneIdOption.Value!.Value);

            if (runCreate.PlanIdOption.IsSet)
                writer.WriteNumber("plan_id", runCreate.PlanIdOption.Value!.Value);

            if (runCreate.AuthorIdOption.IsSet)
                writer.WriteNumber("author_id", runCreate.AuthorIdOption.Value!.Value);

            if (runCreate.TagsOption.IsSet)
            {
                writer.WritePropertyName("tags");
                JsonSerializer.Serialize(writer, runCreate.Tags, jsonSerializerOptions);
            }
            if (runCreate.ConfigurationsOption.IsSet)
            {
                writer.WritePropertyName("configurations");
                JsonSerializer.Serialize(writer, runCreate.Configurations, jsonSerializerOptions);
            }
            if (runCreate.CustomFieldOption.IsSet)
            {
                writer.WritePropertyName("custom_field");
                JsonSerializer.Serialize(writer, runCreate.CustomField, jsonSerializerOptions);
            }
            if (runCreate.StartTimeOption.IsSet)
                writer.WriteString("start_time", runCreate.StartTime);

            if (runCreate.EndTimeOption.IsSet)
                writer.WriteString("end_time", runCreate.EndTime);
        }
    }
}
