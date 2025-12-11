using System;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Extensions;
using Qase.ApiClient.V2.Extensions;
using Qase.Csharp.Commons.Clients;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Reporters;
using Qase.Csharp.Commons.Utils;
using Qase.Csharp.Commons.Writers;
using Serilog;

namespace Qase.Csharp.Commons
{
    /// <summary>
    /// Extension methods for IServiceCollection to register Qase services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Qase services to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="config">The Qase configuration</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddQaseServices(this IServiceCollection services, QaseConfig config)
        {
            // Register configuration
            services.AddSingleton(config);

            // Configure Serilog
            var logPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "logs",
                $"{DateTime.Now:yyyyMMdd}.log"
            );

            if (config.Logging.File)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
            }

            var loggerConfiguration = new LoggerConfiguration();

            // Configure console output
            if (config.Logging.Console)
            {
                loggerConfiguration.WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] qase: {Message:lj}{NewLine}{Exception}");
            }

            // Configure file output
            if (config.Logging.File)
            {
                loggerConfiguration.WriteTo.File(
                    logPath,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] qase: {Message:lj}{NewLine}{Exception}");
            }

            if (config.Debug)
            {
                loggerConfiguration.MinimumLevel.Debug();
            }
            else
            {
                loggerConfiguration.MinimumLevel.Information();
            }

            var logger = loggerConfiguration.CreateLogger();

            // Register logging
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(logger, dispose: true);
            });

            // Register API clients
            services.AddSingleton<ClientV1>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ClientV1>>();
                var baseUrl = config.TestOps.Api.Host == "qase.io" ? 
                    "https://api.qase.io/v1" : 
                    $"https://api-{config.TestOps.Api.Host}/v1";
                
                var apiServices = new ServiceCollection();
                apiServices.AddApi(options =>
                {
                    var token = new Qase.ApiClient.V1.Client.ApiKeyToken(config.TestOps.Api.Token!, Qase.ApiClient.V1.Client.ClientUtils.ApiKeyHeader.Token, "");
                    options.AddTokens(token);
                    options.AddApiHttpClients(client =>
                    {
                        client.BaseAddress = new Uri(baseUrl);
                    });
                });
                
                var apiServiceProvider = apiServices.BuildServiceProvider();
                var runApi = apiServiceProvider.GetRequiredService<IRunsApi>();
                var attachmentsApi = apiServiceProvider.GetRequiredService<IAttachmentsApi>();
                var configurationsApi = apiServiceProvider.GetRequiredService<IConfigurationsApi>();
                
                return new ClientV1(logger, config, runApi, attachmentsApi, configurationsApi);
            });

            services.AddSingleton<ClientV2>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ClientV2>>();
                var apiClientV1 = sp.GetRequiredService<ClientV1>();
                var baseUrl = config.TestOps.Api.Host == "qase.io" ? 
                    "https://api.qase.io/v2" : 
                    $"https://api-{config.TestOps.Api.Host}/v2";
                
                // Detect reporter and framework information
                var (reporterName, reporterVersion) = HostInfo.DetectReporter();
                var (framework, frameworkVersion) = HostInfo.DetectTestFramework();
                
                // Build headers
                var xClientHeader = ClientHeadersBuilder.BuildXClientHeader(
                    reporterName: reporterName,
                    reporterVersion: reporterVersion,
                    framework: framework,
                    frameworkVersion: frameworkVersion);
                
                var xPlatformHeader = ClientHeadersBuilder.BuildXPlatformHeader();
                
                var apiServices = new ServiceCollection();
                apiServices.AddApi(options =>
                {
                    var token = new Qase.ApiClient.V2.Client.ApiKeyToken(config.TestOps.Api.Token!, Qase.ApiClient.V2.Client.ClientUtils.ApiKeyHeader.Token, "");
                    options.AddTokens(token);
                    options.AddApiHttpClients(
                        client =>
                        {
                            client.BaseAddress = new Uri(baseUrl);
                        },
                        builder =>
                        {
                            // Add headers to all HTTP requests
                            builder.ConfigureHttpClient(httpClient =>
                            {
                                if (!string.IsNullOrWhiteSpace(xClientHeader))
                                {
                                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Client", xClientHeader);
                                }
                                
                                if (!string.IsNullOrWhiteSpace(xPlatformHeader))
                                {
                                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Platform", xPlatformHeader);
                                }
                            });
                        });
                });
                
                var apiServiceProvider = apiServices.BuildServiceProvider();
                var resultsApi = apiServiceProvider.GetRequiredService<Qase.ApiClient.V2.Api.IResultsApi>();
                
                return new ClientV2(logger, config, apiClientV1, resultsApi);
            });

            services.AddSingleton<IClient>(sp => sp.GetRequiredService<ClientV2>());

            // Register writers
            services.AddSingleton<FileWriter>(sp => new FileWriter(config.Report.Connection.ToString()));

            // Register reporters based on mode
            if (config.Mode == Mode.TestOps)
            {
                services.AddSingleton<IInternalReporter>(sp => 
                    new TestopsReporter(sp.GetRequiredService<ILogger<TestopsReporter>>(), config, sp.GetRequiredService<IClient>()));
            }
            else if (config.Mode == Mode.Report)
            {
                services.AddSingleton<IInternalReporter>(sp =>
                    new FileReporter(sp.GetRequiredService<ILogger<FileReporter>>(), config, sp.GetRequiredService<FileWriter>()));
            }

            // Register fallback reporter if needed
            if (config.Fallback == Mode.TestOps)
            {
                services.AddSingleton<Func<IInternalReporter>>(sp => () => 
                    new TestopsReporter(sp.GetRequiredService<ILogger<TestopsReporter>>(), config, sp.GetRequiredService<IClient>()));
            }
            else if (config.Fallback == Mode.Report)
            {
                services.AddSingleton<Func<IInternalReporter>>(sp => () =>
                    new FileReporter(sp.GetRequiredService<ILogger<FileReporter>>(), config, sp.GetRequiredService<FileWriter>()));
            }

            // Register core reporter
            services.AddSingleton<ICoreReporter>(sp => 
            {
                var logger = sp.GetRequiredService<ILogger<CoreReporter>>();
                var reporter = sp.GetRequiredService<IInternalReporter>();
                var fallbackFactory = sp.GetService<Func<IInternalReporter>>();
                var fallback = fallbackFactory?.Invoke();
                return new CoreReporter(logger, config, reporter, fallback);
            });

            return services;
        }
    }
} 
