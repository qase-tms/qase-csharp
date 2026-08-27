using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Clients;

namespace Qase.Csharp.Commons.Reporters
{
    /// <summary>
    /// Implementation of the Qase TestOps reporter
    /// </summary>
    public class TestopsReporter : IInternalReporter
    {
        private readonly ILogger<TestopsReporter> _logger;
        private readonly QaseConfig _config;
        private readonly IClient _client;
        private long _testRunId;
        private readonly List<TestResult> _results;
        private bool _uploadFailed;

        /// <summary>
        /// Initializes a new instance of the TestopsReporter class
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="config">The configuration for the reporter</param>
        /// <param name="client">The API client to use</param>
        public TestopsReporter(
            ILogger<TestopsReporter> logger,
            QaseConfig config,
            IClient client)
        {
            _logger = logger;
            _config = config;
            _client = client;
            _results = new List<TestResult>();
        }

        /// <inheritdoc />
        public async Task startTestRun()
        {
            if (_config.TestOps.Run?.Id.HasValue == true)
            {
                _testRunId = _config.TestOps.Run!.Id!.Value;
                return;
            }

            _testRunId = await _client.CreateTestRunAsync();
            _config.TestOps.Run!.Id = _testRunId;
            _logger.LogInformation("Test run {RunId} started", _testRunId);
        }

        /// <inheritdoc />
        public async Task completeTestRun()
        {
            if (_uploadFailed)
            {
                // A completed run over partial data looks trustworthy and is not.
                // Leaving it open is the honest signal that something is missing.
                _logger.LogError(
                    "Test run {RunId} left incomplete: {Count} test results could not be uploaded to Qase",
                    _testRunId, _results.Count);
                return;
            }

            if (!_config.TestOps.Run.Complete)
            {
                _logger.LogInformation("Test run {RunId} completion skipped (run.complete=false)", _testRunId);
                return;
            }

            await _client.CompleteTestRunAsync(_testRunId);
            _logger.LogInformation("Test run {RunId} completed", _testRunId);

            // Enable public report if configured
            if (_config.TestOps.ShowPublicReportLink)
            {
                try
                {
                    var publicUrl = await _client.EnablePublicReportAsync(_testRunId);
                    _logger.LogInformation("Public report link: {PublicUrl}", publicUrl);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to generate public report link: {ErrorMessage}", ex.Message);
                }
            }
        }

        /// <inheritdoc />
        public async Task addResult(TestResult result)
        {
            // Apply status filter if configured
            if (_config.TestOps.StatusFilter.Count > 0)
            {
                if (result.Execution?.Status == null)
                {
                    // If execution is null, we don't filter it out - send it
                    _logger.LogDebug("Test result with null execution will be sent (not filtered). Filter: {Filter}", 
                        string.Join(",", _config.TestOps.StatusFilter));
                }
                else
                {
                    var statusString = result.Execution.Status.ToString().ToLowerInvariant();
                    var filterContainsStatus = _config.TestOps.StatusFilter.Any(filter => 
                        string.Equals(filter, statusString, StringComparison.OrdinalIgnoreCase));
                    
                    // If status is in the filter, we filter it out (don't send)
                    if (filterContainsStatus)
                    {
                        _logger.LogDebug("Test result filtered out by status filter. Status: {Status}, Filter: {Filter}", 
                            statusString, string.Join(",", _config.TestOps.StatusFilter));
                        return;
                    }
                }
            }

            _results.Add(result);

            if (result.Execution?.Status == TestResultStatus.Failed)
            {
                _logger.LogInformation(
                    "See why this test failed: {Link}",
                    PrepareLink(result.TestopsIds?.Count > 0 ? result.TestopsIds[0] : null, result.Title!)
                );
            }

            if (_results.Count >= _config.TestOps.Batch.Size)
            {
                await UploadBufferedResultsAsync();
            }
        }

        /// <inheritdoc />
        public Task uploadResults()
        {
            return UploadBufferedResultsAsync();
        }

        /// <summary>
        /// Sends the buffered results in batches, dropping each batch from the
        /// buffer only once the server has accepted it. Whatever is left when an
        /// upload fails is still available to the fallback reporter.
        /// </summary>
        private async Task UploadBufferedResultsAsync()
        {
            var batchSize = _config.TestOps.Batch.Size;

            while (_results.Count > 0)
            {
                var count = batchSize > 0 ? Math.Min(batchSize, _results.Count) : _results.Count;
                var batch = _results.GetRange(0, count);

                try
                {
                    await _client.UploadResultsAsync(_testRunId, batch);
                }
                catch (QaseException ex)
                {
                    _uploadFailed = true;
                    _logger.LogError(
                        ex,
                        "Failed to upload {Count} test results to Qase after all retries; they are lost from this run",
                        _results.Count);
                    throw;
                }

                _results.RemoveRange(0, count);
            }
        }

        /// <inheritdoc />
        public Task<List<TestResult>> getResults()
        {
            return Task.FromResult(_results);
        }

        /// <inheritdoc />
        public Task setResults(List<TestResult> results)
        {
            _results.Clear();
            _results.AddRange(results);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<List<long>> GetTestCaseIdsForExecutionAsync()
        {
            try
            {
                return await _client.GetTestCaseIdsForExecutionAsync();
            }
            catch (QaseException)
            {
                return new List<long>();
            }
        }

        private string PrepareLink(long? id, string title)
        {
            var baseLink = GetBaseUrl(_config.TestOps.Api.Host!) + "/run/" +
                          _config.TestOps.Project + "/dashboard/" + _testRunId +
                          "?source=logs&search=";

            if (id.HasValue)
            {
                return baseLink + _config.TestOps.Project + "-" + id.Value.ToString();
            }

            try
            {
                var encodedTitle = WebUtility.UrlEncode(title);
                return baseLink + encodedTitle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while encoding title");
                return baseLink;
            }
        }

        private string GetBaseUrl(string host)
        {
            if (host == "qase.io")
            {
                return "https://app.qase.io";
            }

            return "https://" + host;
        }
    }
}
