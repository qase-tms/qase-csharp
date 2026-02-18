using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qase.Csharp.Commons.Models.Domain
{
    /// <summary>
    /// Represents a test result
    /// </summary>
    public class TestResult
    {
        /// <summary>
        /// Gets or sets the test ID
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the test title
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the test signature
        /// </summary>
        public string? Signature { get; set; }

        /// <summary>
        /// Gets or sets the run ID
        /// </summary>
        [JsonIgnore]
        public string? RunId { get; set; }

        /// <summary>
        /// Gets or sets the testops IDs
        /// </summary>
        public List<long>? TestopsIds { get; set; }

        /// <summary>
        /// Gets or sets the test execution
        /// </summary>
        public TestResultExecution? Execution { get; set; }

        /// <summary>
        /// Gets or sets the custom fields
        /// </summary>
        public Dictionary<string, string> Fields { get; set; } = new();

        /// <summary>
        /// Gets or sets the attachments
        /// </summary>
        public List<Attachment> Attachments { get; set; } = new();

        /// <summary>
        /// Gets or sets the test steps
        /// </summary>
        public List<StepResult> Steps { get; set; } = new();

        /// <summary>
        /// Gets or sets the test parameters
        /// </summary>
        public Dictionary<string, string> Params { get; set; } = new();

        /// <summary>
        /// Gets or sets the parameter groups
        /// </summary>
        public List<List<string>> ParamGroups { get; set; } = new();

        /// <summary>
        /// Gets or sets the test relations
        /// </summary>
        public Relations? Relations { get; set; }

        /// <summary>
        /// Gets or sets whether the test is muted
        /// </summary>
        public bool Muted { get; set; }

        /// <summary>
        /// Gets or sets the test message
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets whether the test should be ignored
        /// </summary>
        [JsonIgnore]
        public bool Ignore { get; set; }

        /// <summary>
        /// Initializes a new instance of the TestResult class
        /// </summary>
        public TestResult()
        {
            Relations = new Relations();
            Execution = new TestResultExecution();
        }

        /// <summary>
        /// Returns a string representation of the test result
        /// </summary>
        /// <returns>A JSON string representing the test result</returns>
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }
    }
} 
