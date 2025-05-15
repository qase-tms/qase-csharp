namespace Qase.Csharp.Commons.Models.Domain
{
    /// <summary>
    /// Represents a test result execution
    /// </summary>
    public class TestResultExecution
    {
        /// <summary>
        /// Gets or sets the start time in milliseconds
        /// </summary>
        public long? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the test status
        /// </summary>
        public TestResultStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the end time in milliseconds
        /// </summary>
        public long? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration in milliseconds
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// Gets or sets the stack trace
        /// </summary>
        public string? Stacktrace { get; set; }

        /// <summary>
        /// Gets or sets the thread name
        /// </summary>
        public string? Thread { get; set; }
    }
} 
