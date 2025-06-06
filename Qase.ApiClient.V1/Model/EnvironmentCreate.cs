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
    /// EnvironmentCreate
    /// </summary>
    public partial class EnvironmentCreate : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentCreate" /> class.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="slug">slug</param>
        /// <param name="description">description</param>
        /// <param name="host">host</param>
        [JsonConstructor]
        public EnvironmentCreate(string title, string slug, Option<string?> description = default, Option<string?> host = default)
        {
            Title = title;
            Slug = slug;
            DescriptionOption = description;
            HostOption = host;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets Slug
        /// </summary>
        [JsonPropertyName("slug")]
        public string Slug { get; set; }

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
        /// Used to track the state of Host
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> HostOption { get; private set; }

        /// <summary>
        /// Gets or Sets Host
        /// </summary>
        [JsonPropertyName("host")]
        public string? Host { get { return this.HostOption; } set { this.HostOption = new Option<string?>(value); } }

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
            sb.Append("class EnvironmentCreate {\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Slug: ").Append(Slug).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Host: ").Append(Host).Append("\n");
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

            // Slug (string) maxLength
            if (this.Slug != null && this.Slug.Length > 255)
            {
                yield return new ValidationResult("Invalid value for Slug, length must be less than 255.", new [] { "Slug" });
            }

            yield break;
        }
    }

    /// <summary>
    /// A Json converter for type <see cref="EnvironmentCreate" />
    /// </summary>
    public class EnvironmentCreateJsonConverter : JsonConverter<EnvironmentCreate>
    {
        /// <summary>
        /// Deserializes json to <see cref="EnvironmentCreate" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override EnvironmentCreate Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            Option<string?> title = default;
            Option<string?> slug = default;
            Option<string?> description = default;
            Option<string?> host = default;

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
                        case "slug":
                            slug = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "description":
                            description = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "host":
                            host = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!title.IsSet)
                throw new ArgumentException("Property is required for class EnvironmentCreate.", nameof(title));

            if (!slug.IsSet)
                throw new ArgumentException("Property is required for class EnvironmentCreate.", nameof(slug));

            if (title.IsSet && title.Value == null)
                throw new ArgumentNullException(nameof(title), "Property is not nullable for class EnvironmentCreate.");

            if (slug.IsSet && slug.Value == null)
                throw new ArgumentNullException(nameof(slug), "Property is not nullable for class EnvironmentCreate.");

            if (description.IsSet && description.Value == null)
                throw new ArgumentNullException(nameof(description), "Property is not nullable for class EnvironmentCreate.");

            if (host.IsSet && host.Value == null)
                throw new ArgumentNullException(nameof(host), "Property is not nullable for class EnvironmentCreate.");

            return new EnvironmentCreate(title.Value!, slug.Value!, description, host);
        }

        /// <summary>
        /// Serializes a <see cref="EnvironmentCreate" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="environmentCreate"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, EnvironmentCreate environmentCreate, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            WriteProperties(writer, environmentCreate, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="EnvironmentCreate" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="environmentCreate"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, EnvironmentCreate environmentCreate, JsonSerializerOptions jsonSerializerOptions)
        {
            if (environmentCreate.Title == null)
                throw new ArgumentNullException(nameof(environmentCreate.Title), "Property is required for class EnvironmentCreate.");

            if (environmentCreate.Slug == null)
                throw new ArgumentNullException(nameof(environmentCreate.Slug), "Property is required for class EnvironmentCreate.");

            if (environmentCreate.DescriptionOption.IsSet && environmentCreate.Description == null)
                throw new ArgumentNullException(nameof(environmentCreate.Description), "Property is required for class EnvironmentCreate.");

            if (environmentCreate.HostOption.IsSet && environmentCreate.Host == null)
                throw new ArgumentNullException(nameof(environmentCreate.Host), "Property is required for class EnvironmentCreate.");

            writer.WriteString("title", environmentCreate.Title);

            writer.WriteString("slug", environmentCreate.Slug);

            if (environmentCreate.DescriptionOption.IsSet)
                writer.WriteString("description", environmentCreate.Description);

            if (environmentCreate.HostOption.IsSet)
                writer.WriteString("host", environmentCreate.Host);
        }
    }
}
