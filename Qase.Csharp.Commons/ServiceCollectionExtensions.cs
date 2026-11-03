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
            // Serilog's daily rolling inserts the date itself: logs/qase-20250101.log
            var logPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "logs",
                "qase-.log"
            );

            if (config.Logging.File)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
            }

            var loggerConfiguration = new LoggerConfiguration()
                .Destructure.ByTransforming<ApiConfig>(api => new
                {
                    Token = MaskSecret(api.Token),
                    api.Host,
                    api.Timeout,
                    api.Retries,
                    api.RetryBackoff
                });

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
                var userAgent = ClientHeadersBuilder.BuildUserAgentHeader();

                apiServices.AddApi(options =>
                {
                    var token = new Qase.ApiClient.V1.Client.ApiKeyToken(config.TestOps.Api.Token!, Qase.ApiClient.V1.Client.ClientUtils.ApiKeyHeader.Token, "");
                    options.AddTokens(token);
                    options.AddApiHttpClients(
                        client =>
                        {
                            client.BaseAddress = new Uri(baseUrl);
                        },
                        builder =>
                        {
                            builder.ConfigureHttpClient(httpClient =>
                            {
                                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
                                // No retry here: POST /run carries no idempotency key, so a
                                // replay of a request whose response was lost would create a
                                // second test run.
                                httpClient.Timeout = QaseHttpPolicies.ResolveTimeout(config.TestOps.Api.Timeout);
                            });
                        });
                });

                var apiServiceProvider = apiServices.BuildServiceProvider();
                var runApi = apiServiceProvider.GetRequiredService<IRunsApi>();
                var attachmentsApi = apiServiceProvider.GetRequiredService<IAttachmentsApi>();
                var configurationsApi = apiServiceProvider.GetRequiredService<IConfigurationsApi>();
                var plansApi = apiServiceProvider.GetRequiredService<IPlansApi>();

                return new ClientV1(logger, config, runApi, attachmentsApi, configurationsApi, plansApi);
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
                    var userAgentV2 = ClientHeadersBuilder.BuildUserAgentHeader();

                    options.AddApiHttpClients(
                        client =>
                        {
                            client.BaseAddress = new Uri(baseUrl);
                        },
                        builder =>
                        {
                            // Results carry result.Id as an idempotency key, so replaying a
                            // batch the server already accepted is deduplicated rather than
                            // duplicated. That is what makes retrying safe here.
                            builder.AddQaseResultsPolicies(config.TestOps.Api, logger);

                            // Add headers to all HTTP requests
                            builder.ConfigureHttpClient(httpClient =>
                            {
                                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgentV2);

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
            services.AddSingleton<FileWriter>(sp => new FileWriter(
                sp.GetRequiredService<ILogger<FileWriter>>(),
                config.Report.Connection.Local.Path ?? "./build/qase-report"));

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
                // Off mode registers no IInternalReporter: CoreReporter accepts a null
                // reporter and turns every operation into a no-op.
                var reporter = sp.GetService<IInternalReporter>();
                var fallbackFactory = sp.GetService<Func<IInternalReporter>>();
                var fallback = fallbackFactory?.Invoke();
                return new CoreReporter(logger, config, reporter, fallback);
            });

            return services;
        }

        internal static string? MaskSecret(string? secret)
        {
            if (string.IsNullOrEmpty(secret))
                return secret;

            return secret!.Length <= 8 ? "****" : $"{secret.Substring(0, 4)}****{secret.Substring(secret.Length - 4)}";
        }
    }
} 
