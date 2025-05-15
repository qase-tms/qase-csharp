using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Extensions;
using Qase.ApiClient.V1.Model;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Clients
{
    /// <summary>
    /// Implementation of the Qase API client using V1 API
    /// </summary>
    public class ApiClientV1 : IClient
    {
        private readonly QaseConfig _config;
        private readonly string _url;
        private readonly IRunsApi _runApi;
        private readonly IAttachmentsApi _attachmentsApi;
        private readonly ILogger<ApiClientV1> _logger;

        /// <summary>
        /// Initializes a new instance of the ApiClientV1 class
        /// </summary>
        /// <param name="config">The configuration for the client</param>
        public ApiClientV1(QaseConfig config)
        {
            _config = config;
            _logger = NullLogger<ApiClientV1>.Instance;

            var services = new ServiceCollection();

            var baseUrl = config.TestOps.Api.Host == "qase.io" ? "https://api.qase.io/v1" : $"https://api-{config.TestOps.Api.Host}/v1";
            _url = config.TestOps.Api.Host == "qase.io" ? "https://app.qase.io/" : $"https://app-{config.TestOps.Api.Host}/";

            services.AddApi(options =>
            {
                ApiKeyToken token = new(config.TestOps.Api.Token!, ClientUtils.ApiKeyHeader.Token, "");
                options.AddTokens(token);
                options.AddApiHttpClients(client =>
                {
                    client.BaseAddress = new Uri(baseUrl);
                });
            });

            var serviceProvider = services.BuildServiceProvider();
            _runApi = serviceProvider.GetRequiredService<IRunsApi>();
            _attachmentsApi = serviceProvider.GetRequiredService<IAttachmentsApi>();


            _logger.LogDebug("ApiClientV1 initialized with base URL: {BaseUrl}", baseUrl);
        }

        /// <inheritdoc />
        public async Task<long> CreateTestRunAsync()
        {
            var utcTime = DateTime.UtcNow.AddSeconds(-10).ToString("yyyy-MM-dd HH:mm:ss");

            var runData = new RunCreate(title: _config.TestOps.Run.Title ?? "Automated Test Run",
                description: _config.TestOps.Run.Description,
                isAutotest: true,
                startTime: utcTime,
                environmentSlug: _config.Environment
            );

            try
            {
                _logger.LogDebug("Sending request to create test run with data: {@RunData}", runData);
                var resp = await _runApi.CreateRunAsync(_config.TestOps.Project!, runData);

                if (resp.IsSuccessStatusCode)
                {
                    var runId = resp.Ok()?.Result?.Id ?? throw new QaseException("Failed to create test run, invalid response");
                    return runId;
                }
                else
                {
                    throw new QaseException($"Failed to create test run: {resp.ReasonPhrase}");
                }
            }
            catch (Exception ex) when (!(ex is QaseException))
            {
                throw new QaseException($"Failed to create test run: {ex.Message}", ex);
            }
        }

        /// <inheritdoc />
        public async Task CompleteTestRunAsync(long runId)
        {
            _logger.LogDebug("Completing test run with ID: {RunId}", runId);

            try
            {
                var resp = await _runApi.CompleteRunAsync(_config.TestOps.Project!, (int)runId);
                if (resp.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Test run completed successfully. Link: {Url}run/{Project}/dashboard/{RunId}",
                        _url, _config.TestOps.Project, runId);
                    return;
                }
                else
                {
                    throw new QaseException($"Failed to complete test run: {resp.ReasonPhrase}");
                }
            }
            catch (Exception ex) when (!(ex is QaseException))
            {
                throw new QaseException($"Failed to complete test run: {ex.Message}", ex);
            }
        }

        /// <inheritdoc />
        public Task UploadResultsAsync(long runId, List<TestResult> results)
        {
            throw new NotImplementedException("Use ApiClientV2 for uploading results");
        }

        /// <inheritdoc />
        public async Task<List<long>> GetTestCaseIdsForExecutionAsync()
        {
            if (!_config.TestOps.Plan.Id.HasValue || _config.TestOps.Plan.Id.Value == 0)
            {
                _logger.LogDebug("No plan ID specified, returning empty list of test case IDs");
                return new List<long>();
            }

            var planId = _config.TestOps.Plan.Id.Value;
            _logger.LogDebug("Getting test case IDs for plan: {PlanId}", planId);

            try
            {
                var resp = await _runApi.GetRunAsync(_config.TestOps.Project!, (int)planId);
                if (resp.IsSuccessStatusCode)
                {
                    return resp.Ok()?.Result?.Cases?.ToList() ?? new List<long>();
                }
                else
                {
                    throw new QaseException($"Failed to get test case ids for execution: {resp.ReasonPhrase}");
                }
            }
            catch (Exception ex) when (!(ex is QaseException))
            {
                throw new QaseException($"Failed to get test case ids for execution: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Uploads an attachment to Qase TMS
        /// </summary>
        /// <param name="attachment">The attachment to upload</param>
        /// <returns>The hash of the uploaded attachment</returns>
        public async Task<string> UploadAttachmentAsync(Models.Domain.Attachment attachment)
        {
            var tempFiles = new List<string>(1);
            _logger.LogDebug("Uploading attachment: {FileName}", attachment.FileName);

            try
            {
                var filePath = string.Empty;

                if (!string.IsNullOrEmpty(attachment.FilePath) && File.Exists(attachment.FilePath))
                {
                    filePath = attachment.FilePath;
                }
                else if (!string.IsNullOrEmpty(attachment.Content))
                {
                    filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + "_" + attachment.FileName);
                    File.WriteAllText(filePath, attachment.Content);
                    tempFiles.Add(filePath);
                }
                else if (attachment.ContentBytes != null && attachment.ContentBytes.Length > 0)
                {
                    filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + "_" + attachment.FileName);
                    File.WriteAllBytes(filePath, attachment.ContentBytes);
                    tempFiles.Add(filePath);
                }
                else
                {
                    _logger.LogWarning("Attachment has no content");
                    return "";
                }

                var resp = await _attachmentsApi.UploadAttachmentAsync(_config.TestOps.Project!, new List<Stream> { File.OpenRead(filePath) });

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Ok()?.Result?.FirstOrDefault()?.Hash ?? "";
                }
                else
                {
                    _logger.LogError("Failed to upload attachment: {Reason}", resp.ReasonPhrase);
                    return "";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading attachment");
                return "";
            }
            finally
            {
                foreach (var tempFile in tempFiles)
                {
                    try
                    {
                        if (File.Exists(tempFile))
                        {
                            File.Delete(tempFile);
                            _logger.LogDebug("Deleted temporary file: {FilePath}", tempFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete temporary file: {FilePath}", tempFile);
                    }
                }
            }
        }
    }
}
