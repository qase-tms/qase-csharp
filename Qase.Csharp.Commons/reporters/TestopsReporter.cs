using System;
using System.Collections.Generic;
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
            await _client.CompleteTestRunAsync(_testRunId);
            _logger.LogInformation("Test run {RunId} completed", _testRunId);
        }

        /// <inheritdoc />
        public async Task addResult(TestResult result)
        {
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
                await _client.UploadResultsAsync(_testRunId, _results);
                _results.Clear();
            }
        }

        /// <inheritdoc />
        public async Task uploadResults()
        {
            var batchSize = _config.TestOps.Batch.Size;
            var totalResults = _results.Count;

            if (totalResults == 0)
            {
                return;
            }

            if (totalResults <= batchSize)
            {
                await _client.UploadResultsAsync(_testRunId, _results);
                _results.Clear();
                return;
            }

            for (var index = 0; index < totalResults; index += batchSize)
            {
                var end = Math.Min(index + batchSize, totalResults);
                await _client.UploadResultsAsync(_testRunId, _results.GetRange(index, end - index));
            }

            _results.Clear();
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
                          "?source=logs&status=%5B2%5D&search=";

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
