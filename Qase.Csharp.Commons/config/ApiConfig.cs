namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Configuration for API settings
    /// </summary>
    public class ApiConfig
    {
        /// <summary>
        /// Gets or sets the API token
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Gets or sets the API host
        /// </summary>
        public string? Host { get; set; } = "qase.io";

        /// <summary>
        /// Gets or sets the timeout of a single API request, in seconds
        /// </summary>
        public int Timeout { get; set; } = 30;

        /// <summary>
        /// Gets or sets how many times a failed results upload is retried
        /// </summary>
        public int Retries { get; set; } = 3;

        /// <summary>
        /// Gets or sets the base delay of the exponential retry backoff, in seconds
        /// </summary>
        public double RetryBackoff { get; set; } = 1;
    }
}
