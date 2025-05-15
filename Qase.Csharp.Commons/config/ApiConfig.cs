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
    }
}
