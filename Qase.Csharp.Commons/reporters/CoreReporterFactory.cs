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
        private static ICoreReporter? _instance;
        private static readonly object _lock = new object();
        private static IServiceProvider? _serviceProvider;
        private static QaseConfig? _config;

        private CoreReporterFactory()
        {
        }

        /// <summary>
        /// Gets the Qase configuration, loading it once per process.
        /// </summary>
        /// <returns>The loaded configuration</returns>
        public static QaseConfig GetConfig()
        {
            if (_config == null)
            {
                lock (_lock)
                {
                    _config ??= ConfigFactory.LoadConfig();
                }
            }

            return _config;
        }

        /// <summary>
        /// Indicates whether reporting is enabled, i.e. the mode is not off.
        /// Reporters use this to skip wiring themselves up entirely instead of
        /// building a reporter that would discard every result.
        /// </summary>
        /// <returns>True when results should be reported</returns>
        public static bool IsReportingEnabled()
        {
            return GetConfig().Mode != Mode.Off;
        }

        /// <summary>
        /// Clears the cached configuration and reporter so the next call reloads
        /// them. Test-only: production code loads both once per process.
        /// </summary>
        internal static void Reset()
        {
            lock (_lock)
            {
                _instance = null;
                _serviceProvider = null;
                _config = null;
            }
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
                        var config = GetConfig();

                        if (config.Mode == Mode.Off)
                        {
                            // Nothing to report to: hand back a silent no-op reporter
                            // rather than building a container, a Serilog sink and a
                            // log directory whose output no one asked for.
                            return _instance = new CoreReporter(NullLogger<CoreReporter>.Instance, config);
                        }

                        var services = new ServiceCollection();
                        services.AddQaseServices(config);
                        _serviceProvider = services.BuildServiceProvider();

                        var logger = _serviceProvider.GetRequiredService<ILogger<CoreReporter>>();
                        logger.LogDebug("Config: {@Config}", config);

                        _instance = _serviceProvider.GetRequiredService<ICoreReporter>();
                    }
                }
            }
            
            return _instance;
        }
    }
} 
