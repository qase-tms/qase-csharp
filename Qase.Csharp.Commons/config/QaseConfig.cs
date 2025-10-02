using System;
using System.Collections.Generic;

namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Main configuration class for Qase settings
    /// </summary>
    public class QaseConfig
    {
        /// <summary>
        /// Gets or sets the mode
        /// </summary>
        public Mode Mode { get; set; } = Mode.Off;

        /// <summary>
        /// Gets or sets the fallback mode
        /// </summary>
        public Mode Fallback { get; set; } = Mode.Off;

        /// <summary>
        /// Gets or sets the environment
        /// </summary>
        public string? Environment { get; set; }

        /// <summary>
        /// Gets or sets the root suite name
        /// </summary>
        public string? RootSuite { get; set; }

        /// <summary>
        /// Gets or sets the debug mode
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        /// Gets or sets the test ops config
        /// </summary>
        public TestOpsConfig TestOps { get; set; }

        /// <summary>
        /// Gets or sets the report config
        /// </summary>
        public ReportConfig Report { get; set; }

        /// <summary>
        /// Gets or sets the status mapping configuration
        /// </summary>
        public Dictionary<string, string> StatusMapping { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the logging configuration
        /// </summary>
        public LoggingConfig Logging { get; set; } = new LoggingConfig();

        /// <summary>
        /// Initializes a new instance of the QaseConfig class
        /// </summary>
        public QaseConfig()
        {
            TestOps = new TestOpsConfig();
            Report = new ReportConfig();
            Logging = new LoggingConfig();
        }

        public void SetMode(string? mode)
        {
            if (string.IsNullOrEmpty(mode) || mode == "off")
            {
                Mode = Mode.Off;
            }
            else if (mode == "testops")
            {
                Mode = Mode.TestOps;
            }
            else if (mode == "report")
            {
                Mode = Mode.Report;
            }
            else
            {
                throw new ArgumentException("Invalid mode");
            }
        }

        public void SetFallback(string? fallback)
        {
            if (string.IsNullOrEmpty(fallback) || fallback == "off")
            {
                Fallback = Mode.Off;
            }
            else if (fallback == "testops")
            {
                Fallback = Mode.TestOps;
            }
            else if (fallback == "report")
            {
                Fallback = Mode.Report;
            }
            else
            {
                throw new ArgumentException("Invalid fallback");
            }
        }
    }
}
