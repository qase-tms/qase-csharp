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
    /// TestCaseParams
    /// </summary>
    public partial class TestCaseParams : IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestCaseParams" /> class.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="object"></param>
        internal TestCaseParams(Option<List<Object>?> list, Option<Object?> @object)
        {
            ListOption = list;
            ObjectOption = @object;
            OnCreated();
        }

        partial void OnCreated();

        /// <summary>
        /// Used to track the state of List
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<List<Object>?> ListOption { get; private set; }

        /// <summary>
        /// Gets or Sets List
        /// </summary>
        public List<Object>? List { get { return this.ListOption; } set { this.ListOption = new Option<List<Object>?>(value); } }

        /// <summary>
        /// Used to track the state of Object
        /// </summary>
        [JsonIgnore]
        [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
        public Option<Object?> ObjectOption { get; private set; }

        /// <summary>
        /// Gets or Sets Object
        /// </summary>
        public Object? Object { get { return this.ObjectOption; } set { this.ObjectOption = new Option<Object?>(value); } }

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
            sb.Append("class TestCaseParams {\n");
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
    /// A Json converter for type <see cref="TestCaseParams" />
    /// </summary>
    public class TestCaseParamsJsonConverter : JsonConverter<TestCaseParams>
    {
        /// <summary>
        /// Deserializes json to <see cref="TestCaseParams" />
        /// </summary>
        /// <param name="utf8JsonReader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override TestCaseParams Read(ref Utf8JsonReader utf8JsonReader, Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
        {
            int currentDepth = utf8JsonReader.CurrentDepth;

            if (utf8JsonReader.TokenType != JsonTokenType.StartObject && utf8JsonReader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            JsonTokenType startingTokenType = utf8JsonReader.TokenType;

            List<Object>? list = default;
            Object? varObject = default;

            Utf8JsonReader utf8JsonReaderAnyOf = utf8JsonReader;
            while (utf8JsonReaderAnyOf.Read())
            {
                if (startingTokenType == JsonTokenType.StartObject && utf8JsonReaderAnyOf.TokenType == JsonTokenType.EndObject && currentDepth == utf8JsonReaderAnyOf.CurrentDepth)
                    break;

                if (startingTokenType == JsonTokenType.StartArray && utf8JsonReaderAnyOf.TokenType == JsonTokenType.EndArray && currentDepth == utf8JsonReaderAnyOf.CurrentDepth)
                    break;

                if (utf8JsonReaderAnyOf.TokenType == JsonTokenType.PropertyName && currentDepth == utf8JsonReaderAnyOf.CurrentDepth - 1)
                {
                    Utf8JsonReader utf8JsonReaderList = utf8JsonReader;
                    ClientUtils.TryDeserialize<List<Object>?>(ref utf8JsonReaderList, jsonSerializerOptions, out list);

                    Utf8JsonReader utf8JsonReaderObject = utf8JsonReader;
                    ClientUtils.TryDeserialize<Object?>(ref utf8JsonReaderObject, jsonSerializerOptions, out varObject);
                }
            }

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
                        default:
                            break;
                    }
                }
            }

            Option<List<Object>?> listParsedValue = list == null
                ? default
                : new Option<List<Object>?>(list);
            Option<Object?> varObjectParsedValue = varObject == null
                ? default
                : new Option<Object?>(varObject);

            return new TestCaseParams(listParsedValue, varObjectParsedValue);
        }

        /// <summary>
        /// Serializes a <see cref="TestCaseParams" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="testCaseParams"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, TestCaseParams testCaseParams, JsonSerializerOptions jsonSerializerOptions)
        {
            writer.WriteStartObject();

            if (testCaseParams.ListOption.IsSet && testCaseParams.ListOption.Value != null)

            if (testCaseParams.ObjectOption.IsSet && testCaseParams.ObjectOption.Value != null)

            WriteProperties(writer, testCaseParams, jsonSerializerOptions);
            writer.WriteEndObject();
        }

        /// <summary>
        /// Serializes the properties of <see cref="TestCaseParams" />
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="testCaseParams"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteProperties(Utf8JsonWriter writer, TestCaseParams testCaseParams, JsonSerializerOptions jsonSerializerOptions)
        {

        }
    }
}
