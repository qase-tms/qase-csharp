using System.Collections.Generic;
using System.Linq;

namespace Qase.Csharp.Commons.Models.Report
{
    /// <summary>
    /// Root report model per root.yaml schema
    /// </summary>
    public class Run
    {
        /// <summary>
        /// Gets or sets the run title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the execution timing information
        /// </summary>
        public RunExecution Execution { get; set; }

        /// <summary>
        /// Gets or sets the test outcome statistics
        /// </summary>
        public RunStats Stats { get; set; }

        /// <summary>
        /// Gets or sets the list of test results
        /// </summary>
        public List<ShortResult> Results { get; set; }

        /// <summary>
        /// Gets or sets the list of thread names
        /// </summary>
        public List<string>? Threads { get; set; }

        /// <summary>
        /// Gets or sets the list of suite names
        /// </summary>
        public List<string>? Suites { get; set; }

        /// <summary>
        /// Gets or sets the environment name
        /// </summary>
        public string? Environment { get; set; }

        /// <summary>
        /// Gets or sets the host data
        /// </summary>
        public Dictionary<string, string>? HostData { get; set; }

        /// <summary>
        /// Initializes a new instance of the Run class
        /// </summary>
        /// <param name="title">The run title</param>
        /// <param name="startTime">The start time in milliseconds since Unix epoch</param>
        /// <param name="endTime">The end time in milliseconds since Unix epoch</param>
        /// <param name="environment">The environment name</param>
        public Run(string title, long startTime, long endTime, string? environment)
        {
            Title = title;
            Execution = new RunExecution(startTime, endTime);
            Stats = new RunStats();
            Results = new List<ShortResult>();
            Environment = environment;
        }

        /// <summary>
        /// Adds a test result to the run
        /// </summary>
        /// <param name="id">The result ID</param>
        /// <param name="title">The test title</param>
        /// <param name="status">The test status</param>
        /// <param name="duration">The test duration in milliseconds</param>
        /// <param name="thread">The thread name</param>
        /// <param name="muted">Whether the test is muted</param>
        public void AddResult(string id, string title, string status, int duration, string? thread, bool muted)
        {
            var result = new ShortResult(id, title, status, duration, thread);
            Results.Add(result);
            Execution.Track(result);
            Stats.Track(status, muted);
        }

        /// <summary>
        /// Computes the list of distinct thread names from the results
        /// </summary>
        public void ComputeThreads()
        {
            Threads = Results
                .Where(r => r.Thread != null)
                .Select(r => r.Thread!)
                .Distinct()
                .ToList();
        }
    }
}
