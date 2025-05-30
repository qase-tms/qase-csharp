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
    /// ProjectCounts
    /// </summary>
    public partial class ProjectCounts : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectCounts" /> class.
        /// </summary>
        /// <param name="cases">cases</param>
        /// <param name="suites">suites</param>
        /// <param name="milestones">milestones</param>
        /// <param name="runs">runs</param>
        /// <param name="defects">defects</param>
        [JsonConstructor]
        public ProjectCounts(Option<int?> cases = default, Option<int?> suites = default, Option<int?> milestones = default, Option<ProjectCountsRuns?> runs = default, Option<ProjectCountsDefects?> defects = default)
        {
            CasesOption = cases;
            SuitesOption = suites;
            MilestonesOption = milestones;
            RunsOption = runs;
            DefectsOption = defects;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Used to track the state of Cases
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<int?> CasesOption { get; private set; }

        /// <summary>
        /// Gets or Sets Cases
        /// </summary>
        [JsonPropertyName("cases")]
        public int? Cases { get { return this.CasesOption; } set { this.CasesOption = new Option<int?>(value); } }

        /// <summary>
        /// Used to track the state of Suites
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<int?> SuitesOption { get; private set; }

        /// <summary>
        /// Gets or Sets Suites
        /// </summary>
        [JsonPropertyName("suites")]
        public int? Suites { get { return this.SuitesOption; } set { this.SuitesOption = new Option<int?>(value); } }

        /// <summary>
        /// Used to track the state of Milestones
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<int?> MilestonesOption { get; private set; }

        /// <summary>
        /// Gets or Sets Milestones
        /// </summary>
        [JsonPropertyName("milestones")]
        public int? Milestones { get { return this.MilestonesOption; } set { this.MilestonesOption = new Option<int?>(value); } }

        /// <summary>
        /// Used to track the state of Runs
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<ProjectCountsRuns?> RunsOption { get; private set; }

        /// <summary>
        /// Gets or Sets Runs
        /// </summary>
        [JsonPropertyName("runs")]
        public ProjectCountsRuns? Runs { get { return this.RunsOption; } set { this.RunsOption = new Option<ProjectCountsRuns?>(value); } }

        /// <summary>
        /// Used to track the state of Defects
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<ProjectCountsDefects?> DefectsOption { get; private set; }

        /// <summary>
        /// Gets or Sets Defects
        /// </summary>
        [JsonPropertyName("defects")]
        public ProjectCountsDefects? Defects { get { return this.DefectsOption; } set { this.DefectsOption = new Option<ProjectCountsDefects?>(value); } }

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
            sb.Append("class ProjectCounts {\n");
            sb.Append("  Cases: ").Append(Cases).Append("\n");
            sb.Append("  Suites: ").Append(Suites).Append("\n");
            sb.Append("  Milestones: ").Append(Milestones).Append("\n");
            sb.Append("  Runs: ").Append(Runs).Append("\n");
            sb.Append("  Defects: ").Append(Defects).Append("\n");
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
    /// A Json converter for type <see cref="ProjectCounts" />
    /// </summary>
    public class ProjectCountsJsonConverter : JsonConverter<ProjectCounts>
    {
        /// <summary>
        /// Deserializes json to <see cref="ProjectCounts" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override ProjectCounts Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            Option<int?> cases = default;
            Option<int?> suites = default;
            Option<int?> milestones = default;
            Option<ProjectCountsRuns?> runs = default;
            Option<ProjectCountsDefects?> defects = default;

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
                        case "cases":
                            cases = new Option<int?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (int?)null : utf8JsonReader.GetInt32());
                            break;
                        case "suites":
                            suites = new Option<int?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (int?)null : utf8JsonReader.GetInt32());
                            break;
                        case "milestones":
                            milestones = new Option<int?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (int?)null : utf8JsonReader.GetInt32());
                            break;
                        case "runs":
                            runs = new Option<ProjectCountsRuns?>(JsonSerializer.Deserialize<ProjectCountsRuns>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        case "defects":
                            defects = new Option<ProjectCountsDefects?>(JsonSerializer.Deserialize<ProjectCountsDefects>(ref utf8JsonReader, jsonSerializerOptions)!);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (cases.IsSet && cases.Value == null)
                throw new ArgumentNullException(nameof(cases), "Property is not nullable for class ProjectCounts.");

            if (suites.IsSet && suites.Value == null)
                throw new ArgumentNullException(nameof(suites), "Property is not nullable for class ProjectCounts.");

            if (milestones.IsSet && milestones.Value == null)
                throw new ArgumentNullException(nameof(milestones), "Property is not nullable for class ProjectCounts.");

            if (runs.IsSet && runs.Value == null)
                throw new ArgumentNullException(nameof(runs), "Property is not nullable for class ProjectCounts.");

            if (defects.IsSet && defects.Value == null)
                throw new ArgumentNullException(nameof(defects), "Property is not nullable for class ProjectCounts.");

            return new ProjectCounts(cases, suites, milestones, runs, defects);
        }

        /// <summary>
        /// Serializes a <see cref="ProjectCounts" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="projectCounts"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, ProjectCounts projectCounts, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            WriteProperties(writer, projectCounts, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="ProjectCounts" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="projectCounts"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, ProjectCounts projectCounts, JsonSerializerOptions jsonSerializerOptions)
        {
            if (projectCounts.RunsOption.IsSet && projectCounts.Runs == null)
                throw new ArgumentNullException(nameof(projectCounts.Runs), "Property is required for class ProjectCounts.");

            if (projectCounts.DefectsOption.IsSet && projectCounts.Defects == null)
                throw new ArgumentNullException(nameof(projectCounts.Defects), "Property is required for class ProjectCounts.");

            if (projectCounts.CasesOption.IsSet)
                writer.WriteNumber("cases", projectCounts.CasesOption.Value!.Value);

            if (projectCounts.SuitesOption.IsSet)
                writer.WriteNumber("suites", projectCounts.SuitesOption.Value!.Value);

            if (projectCounts.MilestonesOption.IsSet)
                writer.WriteNumber("milestones", projectCounts.MilestonesOption.Value!.Value);

            if (projectCounts.RunsOption.IsSet)
            {
                writer.WritePropertyName("runs");
                JsonSerializer.Serialize(writer, projectCounts.Runs, jsonSerializerOptions);
            }
            if (projectCounts.DefectsOption.IsSet)
            {
                writer.WritePropertyName("defects");
                JsonSerializer.Serialize(writer, projectCounts.Defects, jsonSerializerOptions);
            }
        }
    }
}
