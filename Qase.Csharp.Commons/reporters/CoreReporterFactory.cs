using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;

namespace Qase.Csharp.Commons.Reporters
{
    /// <summary>
    /// Factory class for creating and managing CoreReporter instances
    /// </summary>
    public class CoreReporterFactory
    {
        private static readonly ILogger<CoreReporterFactory> _logger = NullLogger<CoreReporterFactory>.Instance;
        private static CoreReporter? _instance;
        private static readonly object _lock = new object();

        private CoreReporterFactory()
        {
        }

        /// <summary>
        /// Gets the singleton instance of CoreReporter
        /// </summary>
        /// <returns>The CoreReporter instance</returns>
        public static CoreReporter GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        var config = ConfigFactory.LoadConfig();
                        
                        if (config.Debug)
                        {
                            // TODO: Implement debug logging level setting
                            // For now, we'll just log that debug is enabled
                            _logger.LogDebug("Debug mode is enabled");
                        }
                        
                        _logger.LogDebug("Qase config: {Config}", config);
                        
                        var hostInfoCollector = new HostInfo();
                        var hostInfo = hostInfoCollector.GetHostInfo(typeof(CoreReporterFactory).Assembly.GetName().Version?.ToString());
                        _logger.LogDebug("Using host info: {HostInfo}", hostInfo);
                        
                        _instance = new CoreReporter(config);
                    }
                }
            }
            
            return _instance;
        }
    }
} 
