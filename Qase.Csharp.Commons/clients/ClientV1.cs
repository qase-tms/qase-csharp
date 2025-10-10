using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Model;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;

namespace Qase.Csharp.Commons.Clients
{
    /// <summary>
    /// Implementation of the Qase API client using V1 API
    /// </summary>
    public class ClientV1 : IClient
    {
        private readonly QaseConfig _config;
        private readonly string _url;
        private readonly IRunsApi _runApi;
        private readonly IAttachmentsApi _attachmentsApi;
        private readonly IConfigurationsApi _configurationsApi;
        private readonly ILogger<ClientV1> _logger;

        /// <summary>
        /// Initializes a new instance of the ApiClientV1 class
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="config">The configuration for the client</param>
        /// <param name="runApi">The runs API client</param>
        /// <param name="attachmentsApi">The attachments API client</param>
        public ClientV1(
            ILogger<ClientV1> logger,
            QaseConfig config,
            IRunsApi runApi,
            IAttachmentsApi attachmentsApi,
            IConfigurationsApi configurationsApi)
        {
            _logger = logger;
            _config = config;
            _runApi = runApi;
            _attachmentsApi = attachmentsApi;
            _configurationsApi = configurationsApi;
            _url = config.TestOps.Api.Host == "qase.io" ? "https://app.qase.io/" : $"https://app-{config.TestOps.Api.Host}/";
        }

        /// <inheritdoc />
        public async Task<long> CreateTestRunAsync()
        {
            var utcTime = DateTime.UtcNow.AddSeconds(-10).ToString("yyyy-MM-dd HH:mm:ss");

            var runData = new RunCreate(title: _config.TestOps.Run.Title ?? "Automated Test Run " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                description: _config.TestOps.Run.Description ?? "Automated Test Run " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                isAutotest: true,
                startTime: utcTime
            );

            if (!string.IsNullOrEmpty(_config.Environment))
            {
                runData.EnvironmentSlug = _config.Environment;
            }

            if (_config.TestOps.Run.Tags.Count > 0)
            {
                runData.Tags = _config.TestOps.Run.Tags;
            }

            if (_config.TestOps.Configurations.Values.Count() > 0)
            {
                var configurationIds = await GetConfigurationIdsAsync();
                if (configurationIds.Count > 0)
                {
                    runData.Configurations = configurationIds;
                }
            }

            try
            {
                _logger.LogDebug("Sending request to create test run with data: {@RunData}", runData);
                var resp = await _runApi.CreateRunAsync(_config.TestOps.Project!, runData);

                if (resp.IsSuccessStatusCode)
                {
                    var runId = resp.Ok()?.Result?.Id ?? throw new QaseException("Failed to create test run, invalid response");

                    // Attach external link if configured
                    if (_config.TestOps.Run.ExternalLink != null)
                    {
                        await AttachExternalLinkAsync(runId);
                    }

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
            var fileName = attachment.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                if (attachment.FilePath != null && File.Exists(attachment.FilePath))
                {
                    fileName = Path.GetFileName(attachment.FilePath);
                }
                else
                {
                    fileName = Path.GetRandomFileName();
                }
            }

            _logger.LogDebug("Uploading attachment: {FileName}", fileName);

            try
            {
                var filePath = string.Empty;

                if (!string.IsNullOrEmpty(attachment.FilePath) && File.Exists(attachment.FilePath))
                {
                    filePath = attachment.FilePath;
                }
                else if (!string.IsNullOrEmpty(attachment.Content))
                {
                    filePath = Path.Combine(Path.GetTempPath(), fileName);
                    File.WriteAllText(filePath, attachment.Content);
                    tempFiles.Add(filePath);
                }
                else if (attachment.ContentBytes != null && attachment.ContentBytes.Length > 0)
                {
                    filePath = Path.Combine(Path.GetTempPath(), fileName);
                    File.WriteAllBytes(filePath, attachment.ContentBytes);
                    tempFiles.Add(filePath);
                }
                else
                {
                    _logger.LogWarning("Attachment has no content");
                    return "";
                }

                var resp = await _attachmentsApi.UploadAttachmentAsync(_config.TestOps.Project!, new List<(Stream Stream, string FileName)> { (File.OpenRead(filePath), fileName!) });

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Ok()?.Result?.FirstOrDefault()?.Hash ?? "";
                }
                else
                {
                    _logger.LogError("Failed to upload attachment: {Reason}", resp.ReasonPhrase + " " + resp.RawContent);
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

        /// <summary>
        /// Gets configuration IDs by resolving configuration names and values
        /// </summary>
        /// <returns>List of configuration IDs</returns>
        private async Task<List<long>> GetConfigurationIdsAsync()
        {
            if (_config.TestOps.Configurations.Values.Count() == 0)
            {
                return new List<long>();
            }

            var configurationIds = new List<long>();

            try
            {
                // Get all configuration groups
                var resp = await _configurationsApi.GetConfigurationsAsync(_config.TestOps.Project!);
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to get configurations: {Reason}", resp.ReasonPhrase);
                    return new List<long>();
                }

                var groups = resp.Ok()?.Result?.Entities ?? new List<ConfigurationGroup>();

                foreach (var configItem in _config.TestOps.Configurations.Values)
                {
                    ConfigurationGroup? group = groups.FirstOrDefault(g => g.Title?.Equals(configItem.Name, StringComparison.OrdinalIgnoreCase) == true);

                    // If the group does not exist and creation is allowed, create it
                    if (group == null && _config.TestOps.Configurations.CreateIfNotExists)
                    {
                        var groupCreate = new ConfigurationGroupCreate(configItem.Name);
                        var groupResp = await _configurationsApi.CreateConfigurationGroupAsync(_config.TestOps.Project!, groupCreate);
                        if (groupResp.IsSuccessStatusCode)
                        {
                            var newGroupId = groupResp.Ok()?.Result?.Id;
                            if (newGroupId.HasValue)
                            {
                                group = new ConfigurationGroup { Id = newGroupId.Value, Title = configItem.Name, Configurations = new List<ModelConfiguration>() };
                                groups.Add(group);
                                _logger.LogDebug("Created new configuration group: {Name} with ID: {Id}", configItem.Name, newGroupId.Value);
                            }
                        }
                    }

                    if (group == null)
                    {
                        _logger.LogWarning("Configuration group {Name} not found and createIfNotExists is false", configItem.Name);
                        continue;
                    }

                    // In the group, search for configuration by value
                    var conf = group.Configurations?.FirstOrDefault(c => c.Title?.Equals(configItem.Value, StringComparison.OrdinalIgnoreCase) == true);

                    // If the configuration does not exist and creation is allowed, create it
                    if (conf == null && _config.TestOps.Configurations.CreateIfNotExists)
                    {
                        var confCreate = new ConfigurationCreate(configItem.Value, (int)group.Id!);
                        var confResp = await _configurationsApi.CreateConfigurationAsync(_config.TestOps.Project!, confCreate);
                        if (confResp.IsSuccessStatusCode)
                        {
                            var newConfId = confResp.Ok()?.Result?.Id;
                            if (newConfId.HasValue)
                            {
                                conf = new ModelConfiguration { Id = newConfId.Value, Title = configItem.Value };
                                group.Configurations ??= new List<ModelConfiguration>();
                                group.Configurations.Add(conf);
                                _logger.LogDebug("Created new configuration: {Value} in group {Name} with ID: {Id}", configItem.Value, configItem.Name, newConfId.Value);
                            }
                        }
                    }

                    if (conf == null)
                    {
                        _logger.LogWarning("Configuration {Value} in group {Name} not found and createIfNotExists is false", configItem.Value, configItem.Name);
                        continue;
                    }

                    configurationIds.Add(conf.Id ?? 0);
                }

                return configurationIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get configuration IDs");
                return new List<long>();
            }
        }

        /// <inheritdoc />
        public virtual async Task<string> EnablePublicReportAsync(long runId)
        {
            _logger.LogDebug("Enabling public report for run {RunId}", runId);

            try
            {
                var runPublic = new RunPublic(status: true);
                var resp = await _runApi.UpdateRunPublicityAsync(_config.TestOps.Project!, (int)runId, runPublic);

                if (resp.IsSuccessStatusCode)
                {
                    var publicUrl = resp.Ok()?.Result?.Url;
                    if (!string.IsNullOrEmpty(publicUrl))
                    {
                        _logger.LogInformation("Public report enabled successfully. URL: {PublicUrl}", publicUrl);
                        return publicUrl;
                    }
                    else
                    {
                        throw new QaseException("Failed to get public report URL from response");
                    }
                }
                else
                {
                    throw new QaseException($"Failed to enable public report: {resp.ReasonPhrase}");
                }
            }
            catch (Exception ex) when (!(ex is QaseException))
            {
                throw new QaseException($"Failed to enable public report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Attaches external link to the test run
        /// </summary>
        /// <param name="runId">The run ID to attach the external link to</param>
        private async Task AttachExternalLinkAsync(long runId)
        {
            try
            {
                _logger.LogDebug("Attaching external link to run {RunId}", runId);

                // Map our enum values to API enum values
                var apiType = _config.TestOps.Run.ExternalLink!.Type == ExternalLinkType.JiraCloud
                    ? RunExternalIssues.TypeEnum.JiraCloud
                    : RunExternalIssues.TypeEnum.JiraServer;

                var externalIssues = new RunExternalIssues(
                    apiType,
                    new List<RunExternalIssuesLinksInner>
                    {
                        new RunExternalIssuesLinksInner(runId, _config.TestOps.Run.ExternalLink.Link)
                    }
                );

                var resp = await _runApi.RunUpdateExternalIssueAsync(_config.TestOps.Project!, externalIssues);

                if (resp.IsSuccessStatusCode)
                {
                    _logger.LogDebug("Successfully attached external link to run {RunId}", runId);
                }
                else
                {
                    _logger.LogWarning("Failed to attach external link to run {RunId}: {ReasonPhrase}", runId, resp.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to attach external link to run {RunId}", runId);
                // Don't throw exception here to avoid breaking the test run creation
            }
        }
    }
}
