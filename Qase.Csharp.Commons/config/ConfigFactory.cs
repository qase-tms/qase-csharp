using System;
using System.IO;
using System.Text.Json;
using Qase.Csharp.Commons.Core;

namespace Qase.Csharp.Commons.Config
{
    /// <summary>
    /// Factory for creating configuration instances
    /// </summary>
    public static class ConfigFactory
    {
        private const string CONFIG_FILE_NAME = "qase.config.json";
        
        /// <summary>
        /// Loads configuration from all available sources
        /// </summary>
        /// <returns>Configuration instance</returns>
        public static QaseConfig LoadConfig()
        {
            try
            {
                // Create a default config instance
                var qaseConfig = new QaseConfig();

                // Load from file
                qaseConfig = LoadFromFile(qaseConfig);

                // Load from environment variables
                qaseConfig = LoadFromEnv(qaseConfig);

                // Load from system properties
                qaseConfig = LoadFromSystemProperties(qaseConfig);

                // Validate configuration
                ValidateConfig(qaseConfig);

                return qaseConfig;
            }
            catch (Exception e)
            {
                throw new QaseException("Failed to load configuration", e);
            }
        }

        /// <summary>
        /// Loads configuration from a file
        /// </summary>
        /// <param name="qaseConfig">Configuration to update</param>
        /// <returns>Updated configuration</returns>
        private static QaseConfig LoadFromFile(QaseConfig qaseConfig)
        {
            if (!CheckFile())
            {
                return qaseConfig;
            }

            try
            {
                var jsonString = File.ReadAllText(CONFIG_FILE_NAME);
                ApplyJsonConfig(qaseConfig, JsonDocument.Parse(jsonString).RootElement);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error reading configuration file: {e.Message}");
            }

            return qaseConfig;
        }

        /// <summary>
        /// Checks if the configuration file exists
        /// </summary>
        /// <returns>True if file exists, false otherwise</returns>
        private static bool CheckFile()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), CONFIG_FILE_NAME);
            return File.Exists(filePath);
        }

        /// <summary>
        /// Loads configuration from environment variables
        /// </summary>
        /// <param name="qaseConfig">Configuration to update</param>
        /// <returns>Updated configuration</returns>
        private static QaseConfig LoadFromEnv(QaseConfig qaseConfig)
        {
            string? envMode = GetEnv("QASE_MODE", null);
            if (envMode != null)
            {
                qaseConfig.SetMode(envMode);
            }

            string? envFallback = GetEnv("QASE_FALLBACK", null);
            if (envFallback != null)
            {
                qaseConfig.SetFallback(envFallback);
            }

            qaseConfig.Environment = GetEnv("QASE_ENVIRONMENT", qaseConfig.Environment);
            qaseConfig.RootSuite = GetEnv("QASE_ROOT_SUITE", qaseConfig.RootSuite);
            qaseConfig.Debug = GetBooleanEnv("QASE_DEBUG", qaseConfig.Debug);

            // TestOps settings
            qaseConfig.TestOps.Project = GetEnv("QASE_TESTOPS_PROJECT", qaseConfig.TestOps.Project);
            qaseConfig.TestOps.Defect = GetBooleanEnv("QASE_TESTOPS_DEFECT", qaseConfig.TestOps.Defect);
            qaseConfig.TestOps.Api.Token = GetEnv("QASE_TESTOPS_API_TOKEN", qaseConfig.TestOps.Api.Token);
            qaseConfig.TestOps.Api.Host = GetEnv("QASE_TESTOPS_API_HOST", qaseConfig.TestOps.Api.Host);
            qaseConfig.TestOps.Run.Title = GetEnv("QASE_TESTOPS_RUN_TITLE", qaseConfig.TestOps.Run.Title);
            qaseConfig.TestOps.Run.Description = GetEnv("QASE_TESTOPS_RUN_DESCRIPTION", qaseConfig.TestOps.Run.Description);
            qaseConfig.TestOps.Run.Id = GetLongEnv("QASE_TESTOPS_RUN_ID", qaseConfig.TestOps.Run.Id);
            qaseConfig.TestOps.Run.Complete = GetBooleanEnv("QASE_TESTOPS_RUN_COMPLETE", qaseConfig.TestOps.Run.Complete);
            qaseConfig.TestOps.Plan.Id = GetLongEnv("QASE_TESTOPS_PLAN_ID", qaseConfig.TestOps.Plan.Id);
            qaseConfig.TestOps.Batch.Size = GetIntEnv("QASE_TESTOPS_BATCH_SIZE", qaseConfig.TestOps.Batch.Size);

            // Report settings
            string? driverStr = GetEnv("QASE_REPORT_DRIVER", null);
            if (driverStr != null)
            {
                qaseConfig.Report.SetDriver(driverStr);
            }

            string? formatStr = GetEnv("QASE_REPORT_CONNECTION_FORMAT", null);
            if (formatStr != null)
            {
                qaseConfig.Report.Connection.Local.SetFormat(formatStr);
            }

            qaseConfig.Report.Connection.Local.Path = GetEnv("QASE_REPORT_CONNECTION_PATH", qaseConfig.Report.Connection.Local.Path);

            return qaseConfig;
        }

        /// <summary>
        /// Loads configuration from system properties
        /// </summary>
        /// <param name="qaseConfig">Configuration to update</param>
        /// <returns>Updated configuration</returns>
        private static QaseConfig LoadFromSystemProperties(QaseConfig qaseConfig)
        {
            // В C# нет прямого эквивалента Java System.getProperty
            // Мы можем использовать AppSettings из .NET Core или другие подходы
            // В данной реализации просто оставим заглушку
            
            // Примечание: если потребуется реальная реализация,
            // ее можно добавить здесь, используя ConfigurationManager или аналогичный механизм
            
            return qaseConfig;
        }

        /// <summary>
        /// Validates the configuration
        /// </summary>
        /// <param name="qaseConfig">Configuration to validate</param>
        private static void ValidateConfig(QaseConfig qaseConfig)
        {
            if ((qaseConfig.Mode == Mode.TestOps || qaseConfig.Fallback == Mode.TestOps) && 
                (string.IsNullOrEmpty(qaseConfig.TestOps?.Project) || string.IsNullOrEmpty(qaseConfig.TestOps?.Api?.Token)))
            {
                Console.Error.WriteLine("Project code and API token are required for TestOps mode");
                qaseConfig.Mode = Mode.Off;
                qaseConfig.Fallback = Mode.Off;
            }
        }

        /// <summary>
        /// Gets an environment variable or returns default value
        /// </summary>
        /// <param name="key">Environment variable name</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Environment variable value or default</returns>
        private static string? GetEnv(string key, string? defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(key);
            return value ?? defaultValue;
        }

        /// <summary>
        /// Gets a boolean environment variable or returns default value
        /// </summary>
        /// <param name="key">Environment variable name</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Environment variable value as boolean or default</returns>
        private static bool GetBooleanEnv(string key, bool defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(key);
            return value != null ? bool.Parse(value) : defaultValue;
        }

        /// <summary>
        /// Gets an integer environment variable or returns default value
        /// </summary>
        /// <param name="key">Environment variable name</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Environment variable value as integer or default</returns>
        private static int GetIntEnv(string key, int defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(key);
            return value != null ? int.Parse(value) : defaultValue;
        }

        /// <summary>
        /// Gets a long environment variable or returns default value
        /// </summary>
        /// <param name="key">Environment variable name</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Environment variable value as long or default</returns>
        private static long? GetLongEnv(string key, long? defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(key);
            return value != null ? long.Parse(value) : defaultValue;
        }

        /// <summary>
        /// Applies JSON configuration to the config object
        /// </summary>
        /// <param name="qaseConfig">Configuration to update</param>
        /// <param name="json">JSON element to read from</param>
        private static void ApplyJsonConfig(QaseConfig qaseConfig, JsonElement json)
        {
            if (json.TryGetProperty("mode", out var modeElement) && modeElement.ValueKind == JsonValueKind.String)
            {
                string? mode = modeElement.GetString();
                if (mode != null)
                {
                    qaseConfig.SetMode(mode);
                }
            }

            if (json.TryGetProperty("fallback", out var fallbackElement) && fallbackElement.ValueKind == JsonValueKind.String)
            {
                string? fallback = fallbackElement.GetString();
                if (fallback != null)
                {
                    qaseConfig.SetFallback(fallback);
                }
            }

            if (json.TryGetProperty("environment", out var envElement) && envElement.ValueKind == JsonValueKind.String)
            {
                qaseConfig.Environment = envElement.GetString();
            }

            if (json.TryGetProperty("rootSuite", out var rootSuiteElement) && rootSuiteElement.ValueKind == JsonValueKind.String)
            {
                qaseConfig.RootSuite = rootSuiteElement.GetString();
            }

            if (json.TryGetProperty("debug", out var debugElement) && debugElement.ValueKind == JsonValueKind.True)
            {
                qaseConfig.Debug = true;
            }

            if (json.TryGetProperty("testops", out var testopsElement))
            {
                if (testopsElement.TryGetProperty("project", out var projectElement) &&
                    projectElement.ValueKind == JsonValueKind.String)
                {
                    qaseConfig.TestOps.Project = projectElement.GetString();
                }

                if (testopsElement.TryGetProperty("defect", out var defectElement) &&
                    defectElement.ValueKind == JsonValueKind.True)
                {
                    qaseConfig.TestOps.Defect = true;
                }

                if (testopsElement.TryGetProperty("api", out var apiElement))
                {
                    if (apiElement.TryGetProperty("token", out var tokenElement) &&
                        tokenElement.ValueKind == JsonValueKind.String)
                    {
                        qaseConfig.TestOps.Api.Token = tokenElement.GetString();
                    }

                    if (apiElement.TryGetProperty("host", out var hostElement) &&
                        hostElement.ValueKind == JsonValueKind.String)
                    {
                        qaseConfig.TestOps.Api.Host = hostElement.GetString();
                    }
                }

                if (testopsElement.TryGetProperty("run", out var runElement))
                {
                    if (runElement.TryGetProperty("id", out var idElement) &&
                        idElement.ValueKind == JsonValueKind.Number)
                    {
                        qaseConfig.TestOps.Run.Id = idElement.GetInt64();
                    }

                    if (runElement.TryGetProperty("title", out var titleElement) &&
                        titleElement.ValueKind == JsonValueKind.String)
                    {
                        qaseConfig.TestOps.Run.Title = titleElement.GetString();
                    }

                    if (runElement.TryGetProperty("description", out var descElement) &&
                        descElement.ValueKind == JsonValueKind.String)
                    {
                        qaseConfig.TestOps.Run.Description = descElement.GetString();
                    }

                    if (runElement.TryGetProperty("complete", out var completeElement) &&
                        completeElement.ValueKind == JsonValueKind.True)
                    {
                        qaseConfig.TestOps.Run.Complete = true;
                    }
                }

                if (testopsElement.TryGetProperty("plan", out var planElement))
                {
                    if (planElement.TryGetProperty("id", out var idElement) &&
                        idElement.ValueKind == JsonValueKind.Number)
                    {
                        qaseConfig.TestOps.Plan.Id = idElement.GetInt64();
                    }
                }

                if (testopsElement.TryGetProperty("batch", out var batchElement))
                {
                    if (batchElement.TryGetProperty("size", out var sizeElement) &&
                        sizeElement.ValueKind == JsonValueKind.Number)
                    {
                        qaseConfig.TestOps.Batch.Size = sizeElement.GetInt32();
                    }
                }
            }

            if (json.TryGetProperty("report", out var reportElement))
            {
                if (reportElement.TryGetProperty("driver", out var driverElement) &&
                    driverElement.ValueKind == JsonValueKind.String)
                {
                    string? driver = driverElement.GetString();
                    if (driver != null)
                    {
                        qaseConfig.Report.SetDriver(driver);
                    }
                }

                if (reportElement.TryGetProperty("connection", out var connectionElement))
                {
                    if (connectionElement.TryGetProperty("local", out var localElement))
                    {
                        if (localElement.TryGetProperty("format", out var formatElement) &&
                            formatElement.ValueKind == JsonValueKind.String)
                        {
                            string? format = formatElement.GetString();
                            if (format != null)
                            {
                                qaseConfig.Report.Connection.Local.SetFormat(format);
                            }
                        }

                        if (localElement.TryGetProperty("path", out var pathElement) &&
                            pathElement.ValueKind == JsonValueKind.String)
                        {
                            qaseConfig.Report.Connection.Local.Path = pathElement.GetString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates a configuration instance
        /// </summary>
        /// <returns>Configuration instance</returns>
        public static QaseConfig Create()
        {
            return LoadConfig();
        }
    }
}

