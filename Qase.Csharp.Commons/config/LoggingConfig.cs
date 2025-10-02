using System;

namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Configuration for logging settings
    /// </summary>
    public class LoggingConfig
    {
        /// <summary>
        /// Gets or sets whether console output is enabled
        /// </summary>
        public bool Console { get; set; } = true;

        /// <summary>
        /// Gets or sets whether file output is enabled
        /// </summary>
        public bool File { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the LoggingConfig class
        /// </summary>
        public LoggingConfig()
        {
        }
    }
}
