using System;
using Microsoft.Extensions.DependencyInjection;
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
        private static ICoreReporter? _instance;
        private static readonly object _lock = new object();
        private static IServiceProvider? _serviceProvider;

        private CoreReporterFactory()
        {
        }

        /// <summary>
        /// Gets the singleton instance of CoreReporter
        /// </summary>
        /// <returns>The CoreReporter instance</returns>
        public static ICoreReporter GetInstance()
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
                        
                        _logger.LogDebug("Qase config: {@Config}", config);
                        
                        var services = new ServiceCollection();
                        services.AddQaseServices(config);
                        _serviceProvider = services.BuildServiceProvider();
                        
                        _instance = _serviceProvider.GetRequiredService<ICoreReporter>();
                    }
                }
            }
            
            return _instance;
        }
    }
} 
