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
    /// Attachment
    /// </summary>
    public partial class Attachment : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Attachment" /> class.
        /// </summary>
        /// <param name="size">size</param>
        /// <param name="mime">mime</param>
        /// <param name="filename">filename</param>
        /// <param name="url">url</param>
        [JsonConstructor]
        public Attachment(Option<int?> size = default, Option<string?> mime = default, Option<string?> filename = default, Option<string?> url = default)
        {
            SizeOption = size;
            MimeOption = mime;
            FilenameOption = filename;
            UrlOption = url;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Used to track the state of Size
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<int?> SizeOption { get; private set; }

        /// <summary>
        /// Gets or Sets Size
        /// </summary>
        [JsonPropertyName("size")]
        public int? Size { get { return this.SizeOption; } set { this.SizeOption = new Option<int?>(value); } }

        /// <summary>
        /// Used to track the state of Mime
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> MimeOption { get; private set; }

        /// <summary>
        /// Gets or Sets Mime
        /// </summary>
        [JsonPropertyName("mime")]
        public string? Mime { get { return this.MimeOption; } set { this.MimeOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of Filename
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> FilenameOption { get; private set; }

        /// <summary>
        /// Gets or Sets Filename
        /// </summary>
        [JsonPropertyName("filename")]
        public string? Filename { get { return this.FilenameOption; } set { this.FilenameOption = new Option<string?>(value); } }

        /// <summary>
        /// Used to track the state of Url
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<string?> UrlOption { get; private set; }

        /// <summary>
        /// Gets or Sets Url
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get { return this.UrlOption; } set { this.UrlOption = new Option<string?>(value); } }

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
            sb.Append("class Attachment {\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  Mime: ").Append(Mime).Append("\n");
            sb.Append("  Filename: ").Append(Filename).Append("\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
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
    /// A Json converter for type <see cref="Attachment" />
    /// </summary>
    public class AttachmentJsonConverter : JsonConverter<Attachment>
    {
        /// <summary>
        /// Deserializes json to <see cref="Attachment" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override Attachment Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            Option<int?> size = default;
            Option<string?> mime = default;
            Option<string?> filename = default;
            Option<string?> url = default;

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
                        case "size":
                            size = new Option<int?>(utf8JsonReader.TokenType == JsonTokenType.Null ? (int?)null : utf8JsonReader.GetInt32());
                            break;
                        case "mime":
                            mime = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "filename":
                            filename = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        case "url":
                            url = new Option<string?>(utf8JsonReader.GetString()!);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (size.IsSet && size.Value == null)
                throw new ArgumentNullException(nameof(size), "Property is not nullable for class Attachment.");

            if (mime.IsSet && mime.Value == null)
                throw new ArgumentNullException(nameof(mime), "Property is not nullable for class Attachment.");

            if (filename.IsSet && filename.Value == null)
                throw new ArgumentNullException(nameof(filename), "Property is not nullable for class Attachment.");

            if (url.IsSet && url.Value == null)
                throw new ArgumentNullException(nameof(url), "Property is not nullable for class Attachment.");

            return new Attachment(size, mime, filename, url);
        }

        /// <summary>
        /// Serializes a <see cref="Attachment" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="attachment"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, Attachment attachment, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            WriteProperties(writer, attachment, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="Attachment" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="attachment"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, Attachment attachment, JsonSerializerOptions jsonSerializerOptions)
        {
            if (attachment.MimeOption.IsSet && attachment.Mime == null)
                throw new ArgumentNullException(nameof(attachment.Mime), "Property is required for class Attachment.");

            if (attachment.FilenameOption.IsSet && attachment.Filename == null)
                throw new ArgumentNullException(nameof(attachment.Filename), "Property is required for class Attachment.");

            if (attachment.UrlOption.IsSet && attachment.Url == null)
                throw new ArgumentNullException(nameof(attachment.Url), "Property is required for class Attachment.");

            if (attachment.SizeOption.IsSet)
                writer.WriteNumber("size", attachment.SizeOption.Value!.Value);

            if (attachment.MimeOption.IsSet)
                writer.WriteString("mime", attachment.Mime);

            if (attachment.FilenameOption.IsSet)
                writer.WriteString("filename", attachment.Filename);

            if (attachment.UrlOption.IsSet)
                writer.WriteString("url", attachment.Url);
        }
    }
}
