namespace Qase.Csharp.Commons.Models.Report
{
    /// <summary>
    /// Tracks timing per root.yaml execution schema
    /// </summary>
    public class RunExecution
    {
        /// <summary>
        /// Gets or sets the start time in milliseconds since Unix epoch
        /// </summary>
        public long StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time in milliseconds since Unix epoch
        /// </summary>
        public long EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration in milliseconds
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the cumulative duration of all tests in milliseconds
        /// </summary>
        public int CumulativeDuration { get; set; }

        /// <summary>
        /// Initializes a new instance of the RunExecution class
        /// </summary>
        /// <param name="startTime">The start time in milliseconds since Unix epoch</param>
        /// <param name="endTime">The end time in milliseconds since Unix epoch</param>
        public RunExecution(long startTime, long endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            Duration = (int)(endTime - startTime);
            CumulativeDuration = 0;
        }

        /// <summary>
        /// Tracks a test result by adding its duration to the cumulative duration
        /// </summary>
        /// <param name="result">The test result</param>
        public void Track(ShortResult result)
        {
            CumulativeDuration += result.Duration;
        }
    }
}
