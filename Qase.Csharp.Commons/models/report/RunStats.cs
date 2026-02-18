namespace Qase.Csharp.Commons.Models.Report
{
    /// <summary>
    /// Tracks test outcome counts per root.yaml stats schema
    /// </summary>
    public class RunStats
    {
        /// <summary>
        /// Gets or sets the total number of tests
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the number of passed tests
        /// </summary>
        public int Passed { get; set; }

        /// <summary>
        /// Gets or sets the number of failed tests
        /// </summary>
        public int Failed { get; set; }

        /// <summary>
        /// Gets or sets the number of skipped tests
        /// </summary>
        public int Skipped { get; set; }

        /// <summary>
        /// Gets or sets the number of blocked tests
        /// </summary>
        public int Blocked { get; set; }

        /// <summary>
        /// Gets or sets the number of invalid tests
        /// </summary>
        public int Invalid { get; set; }

        /// <summary>
        /// Gets or sets the number of muted tests
        /// </summary>
        public int Muted { get; set; }

        /// <summary>
        /// Initializes a new instance of the RunStats class
        /// </summary>
        public RunStats()
        {
            Total = 0;
            Passed = 0;
            Failed = 0;
            Skipped = 0;
            Blocked = 0;
            Invalid = 0;
            Muted = 0;
        }

        /// <summary>
        /// Tracks a test result by incrementing the appropriate counters
        /// </summary>
        /// <param name="status">The test status (passed, failed, skipped, blocked, invalid)</param>
        /// <param name="muted">Whether the test is muted</param>
        public void Track(string status, bool muted)
        {
            Total++;

            switch (status?.ToLowerInvariant())
            {
                case "passed":
                    Passed++;
                    break;
                case "failed":
                    Failed++;
                    break;
                case "skipped":
                    Skipped++;
                    break;
                case "blocked":
                    Blocked++;
                    break;
                case "invalid":
                    Invalid++;
                    break;
            }

            if (muted)
            {
                Muted++;
            }
        }
    }
}
