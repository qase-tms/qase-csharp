using System;
using System.Collections.Generic;

namespace Qase.Csharp.Commons.Models.Domain
{
    /// <summary>
    /// Represents a test step execution
    /// </summary>
    public class StepExecution
    {
        /// <summary>
        /// Gets or sets the step status
        /// </summary>
        public StepResultStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the step comment
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the start time in milliseconds
        /// </summary>
        public long StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time in milliseconds
        /// </summary>
        public long? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration in milliseconds
        /// </summary>
        public long? Duration { get; set; }

        /// <summary>
        /// Gets or sets the step attachments
        /// </summary>
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();

        /// <summary>
        /// Initializes a new instance of the StepExecution class
        /// </summary>
        public StepExecution()
        {
            StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Status = StepResultStatus.Untested;
        }

        /// <summary>
        /// Stops the step execution and calculates duration
        /// </summary>
        public void Stop()
        {
            EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Duration = EndTime - StartTime;
        }
    }
}
