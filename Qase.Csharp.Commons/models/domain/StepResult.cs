using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Qase.Csharp.Commons.Models.Domain
{
    /// <summary>
    /// Represents a test step result
    /// </summary>
    public class StepResult
    {
        /// <summary>
        /// Gets or sets the step ID
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the step type
        /// </summary>
        public string? StepType { get; set; } = "text";

        /// <summary>
        /// Gets or sets the step data
        /// </summary>
        public Data? Data { get; set; }

        /// <summary>
        /// Gets or sets the parent step ID
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the step execution
        /// </summary>
        public StepExecution? Execution { get; set; }

        /// <summary>
        /// Gets or sets the child steps
        /// </summary>
        public List<StepResult> Steps { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the StepResult class
        /// </summary>
        public StepResult()
        {
            Execution = new StepExecution();
            Data = new Data();
        }
    }
} 
