using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Extensions;
using Qase.ApiClient.V1.Model;
using Qase.ApiClient.V2.Api;
using Qase.ApiClient.V2.Client;
using Qase.ApiClient.V2.Extensions;
using Qase.ApiClient.V2.Model;
using Qase.Csharp.Commons.Clients;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Reporters;
using Qase.Csharp.Commons.Writers;

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

            // Register logging
            services.AddLogging(builder =>
            {
                if (config.Debug)
                {
                    builder.SetMinimumLevel(LogLevel.Debug);
                }
            });

            // Register API clients
            services.AddSingleton<ApiClientV1>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ApiClientV1>>();
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
                
                return new ApiClientV1(logger, config, runApi, attachmentsApi);
            });

            services.AddSingleton<ApiClientV2>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ApiClientV2>>();
                var apiClientV1 = sp.GetRequiredService<ApiClientV1>();
                var baseUrl = config.TestOps.Api.Host == "qase.io" ? 
                    "https://api.qase.io/v2" : 
                    $"https://api-{config.TestOps.Api.Host}/v2";
                
                var apiServices = new ServiceCollection();
                apiServices.AddApi(options =>
                {
                    var token = new Qase.ApiClient.V2.Client.ApiKeyToken(config.TestOps.Api.Token!, Qase.ApiClient.V2.Client.ClientUtils.ApiKeyHeader.Token, "");
                    options.AddTokens(token);
                    options.AddApiHttpClients(client =>
                    {
                        client.BaseAddress = new Uri(baseUrl);
                    });
                });
                
                var apiServiceProvider = apiServices.BuildServiceProvider();
                var resultsApi = apiServiceProvider.GetRequiredService<Qase.ApiClient.V2.Api.IResultsApi>();
                
                return new ApiClientV2(logger, config, apiClientV1, resultsApi);
            });

            services.AddSingleton<IClient>(sp => sp.GetRequiredService<ApiClientV2>());

            // Register writers
            services.AddSingleton<FileWriter>(sp => new FileWriter(config.Report.Connection.ToString()));

            // Register reporters based on mode
            if (config.Mode == Mode.TestOps)
            {
                services.AddSingleton<IInternalReporter, TestopsReporter>();
            }
            else if (config.Mode == Mode.Report)
            {
                services.AddSingleton<IInternalReporter, FileReporter>();
            }

            // Register fallback reporter if needed
            if (config.Fallback == Mode.TestOps)
            {
                services.AddSingleton<IInternalReporter, TestopsReporter>(sp => 
                    new TestopsReporter(sp.GetRequiredService<ILogger<TestopsReporter>>(), config, sp.GetRequiredService<IClient>()));
            }
            else if (config.Fallback == Mode.Report)
            {
                services.AddSingleton<IInternalReporter, FileReporter>(sp =>
                    new FileReporter(sp.GetRequiredService<ILogger<FileReporter>>(), config, sp.GetRequiredService<FileWriter>()));
            }

            // Register core reporter
            services.AddSingleton<ICoreReporter, CoreReporter>();

            return services;
        }
    }
} 
