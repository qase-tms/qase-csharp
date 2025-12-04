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

        private const long MaxFileSize = 32 * 1024 * 1024; // 32 MB
        private const long MaxRequestSize = 128 * 1024 * 1024; // 128 MB
        private const int MaxFilesPerRequest = 20;

        /// <summary>
        /// Uploads attachments to Qase TMS
        /// </summary>
        /// <param name="attachments">The attachments to upload</param>
        /// <returns>List of hashes of the uploaded attachments</returns>
        /// <exception cref="QaseException">Thrown when validation fails or upload fails</exception>
        public async Task<List<string>> UploadAttachmentsAsync(List<Models.Domain.Attachment> attachments)
        {
            if (attachments == null || attachments.Count == 0)
            {
                _logger.LogWarning("No attachments provided for upload");
                return new List<string>();
            }

            _logger.LogDebug("Uploading {Count} attachments", attachments.Count);

            var allHashes = new List<string>();
            var tempFiles = new List<string>();
            var fileInfos = new List<(Stream Stream, string FileName, long Size)>();

            try
            {
                // Prepare file streams
                foreach (var attachment in attachments)
                {
                    var fileInfo = PrepareAttachmentFileAsync(attachment, tempFiles);
                    if (fileInfo.HasValue)
                    {
                        fileInfos.Add(fileInfo.Value);
                    }
                }

                // Validate attachments after preparing files
                ValidateAttachments(fileInfos);

                // Group files into batches respecting limits
                var batches = CreateBatches(fileInfos);

                _logger.LogDebug("Created {BatchCount} batches for upload", batches.Count);

                // Upload each batch
                foreach (var batch in batches)
                {
                    var fileStreams = batch.Select(f => (f.Stream, f.FileName)).ToList();
                    var resp = await _attachmentsApi.UploadAttachmentAsync(_config.TestOps.Project!, fileStreams);

                    if (resp.IsSuccessStatusCode)
                    {
                        var hashes = resp.Ok()?.Result?.Select(r => r.Hash).Where(h => !string.IsNullOrEmpty(h)).Select(h => h!).ToList() ?? new List<string>();
                        allHashes.AddRange(hashes);
                        _logger.LogDebug("Successfully uploaded batch of {Count} files, got {HashCount} hashes", batch.Count, hashes.Count);
                    }
                    else
                    {
                        _logger.LogError("Failed to upload attachment batch: {Reason}", resp.ReasonPhrase + " " + resp.RawContent);
                        throw new QaseException($"Failed to upload attachments: {resp.ReasonPhrase}");
                    }
                }

                return allHashes;
            }
            catch (QaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading attachments");
                throw new QaseException($"Failed to upload attachments: {ex.Message}", ex);
            }
            finally
            {
                // Close all streams
                foreach (var fileInfo in fileInfos)
                {
                    try
                    {
                        fileInfo.Stream?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to dispose stream");
                    }
                }

                // Delete temporary files
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
        /// Uploads an attachment to Qase TMS (for backward compatibility)
        /// </summary>
        /// <param name="attachment">The attachment to upload</param>
        /// <returns>The hash of the uploaded attachment</returns>
        public async Task<string> UploadAttachmentAsync(Models.Domain.Attachment attachment)
        {
            var hashes = await UploadAttachmentsAsync(new List<Models.Domain.Attachment> { attachment });
            return hashes.FirstOrDefault() ?? "";
        }

        private void ValidateAttachments(List<(Stream Stream, string FileName, long Size)> fileInfos)
        {
            if (fileInfos.Count > MaxFilesPerRequest)
            {
                throw new QaseException($"Maximum {MaxFilesPerRequest} files per request allowed, but {fileInfos.Count} files provided");
            }

            foreach (var fileInfo in fileInfos)
            {
                if (fileInfo.Size > MaxFileSize)
                {
                    throw new QaseException($"File '{fileInfo.FileName}' size {fileInfo.Size} bytes exceeds maximum allowed size of {MaxFileSize} bytes (32 MB)");
                }
            }
        }

        private (Stream Stream, string FileName, long Size)? PrepareAttachmentFileAsync(Models.Domain.Attachment attachment, List<string> tempFiles)
        {
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

            Stream? stream = null;
            long size = 0;

            if (!string.IsNullOrEmpty(attachment.FilePath) && File.Exists(attachment.FilePath))
            {
                var fileInfo = new FileInfo(attachment.FilePath);
                size = fileInfo.Length;
                stream = File.OpenRead(attachment.FilePath);
            }
            else if (!string.IsNullOrEmpty(attachment.Content))
            {
                var filePath = Path.Combine(Path.GetTempPath(), fileName);
                File.WriteAllText(filePath, attachment.Content);
                tempFiles.Add(filePath);
                var fileInfo = new FileInfo(filePath);
                size = fileInfo.Length;
                stream = File.OpenRead(filePath);
            }
            else if (attachment.ContentBytes != null && attachment.ContentBytes.Length > 0)
            {
                var filePath = Path.Combine(Path.GetTempPath(), fileName);
                File.WriteAllBytes(filePath, attachment.ContentBytes);
                tempFiles.Add(filePath);
                size = attachment.ContentBytes.Length;
                stream = File.OpenRead(filePath);
            }
            else
            {
                _logger.LogWarning("Attachment has no content, skipping");
                return null;
            }

            return (stream, fileName!, size);
        }

        private List<List<(Stream Stream, string FileName, long Size)>> CreateBatches(List<(Stream Stream, string FileName, long Size)> fileInfos)
        {
            var batches = new List<List<(Stream Stream, string FileName, long Size)>>();
            var currentBatch = new List<(Stream Stream, string FileName, long Size)>();
            long currentBatchSize = 0;

            foreach (var fileInfo in fileInfos)
            {
                // Check if single file exceeds request size limit
                if (fileInfo.Size > MaxRequestSize)
                {
                    throw new QaseException($"File '{fileInfo.FileName}' size {fileInfo.Size} bytes exceeds maximum request size of {MaxRequestSize} bytes (128 MB)");
                }

                // Check if adding this file would exceed limits
                if (currentBatch.Count >= MaxFilesPerRequest || 
                    currentBatchSize + fileInfo.Size > MaxRequestSize)
                {
                    // Start a new batch
                    if (currentBatch.Count > 0)
                    {
                        batches.Add(currentBatch);
                        currentBatch = new List<(Stream Stream, string FileName, long Size)>();
                        currentBatchSize = 0;
                    }
                }

                currentBatch.Add(fileInfo);
                currentBatchSize += fileInfo.Size;
            }

            // Add the last batch if it has files
            if (currentBatch.Count > 0)
            {
                batches.Add(currentBatch);
            }

            return batches;
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
