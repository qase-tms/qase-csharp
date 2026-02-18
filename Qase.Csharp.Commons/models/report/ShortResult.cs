namespace Qase.Csharp.Commons.Models.Report
{
    /// <summary>
    /// Summary result for run.json results array per root.yaml results items schema
    /// </summary>
    public class ShortResult
    {
        /// <summary>
        /// Gets or sets the result ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the test title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the test status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the test duration in milliseconds
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the thread name
        /// </summary>
        public string? Thread { get; set; }

        /// <summary>
        /// Initializes a new instance of the ShortResult class
        /// </summary>
        /// <param name="id">The result ID</param>
        /// <param name="title">The test title</param>
        /// <param name="status">The test status</param>
        /// <param name="duration">The test duration in milliseconds</param>
        /// <param name="thread">The thread name</param>
        public ShortResult(string id, string title, string status, int duration, string? thread)
        {
            Id = id;
            Title = title;
            Status = status;
            Duration = duration;
            Thread = thread;
        }
    }
}
