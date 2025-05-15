using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Clients;
using Qase.Csharp.Commons.Writers;

namespace Qase.Csharp.Commons.Reporters
{
    /// <summary>
    /// Core reporter implementation that manages primary and fallback reporters
    /// </summary>
    public class CoreReporter : ICoreReporter
    {
        private readonly ILogger<CoreReporter> _logger;
        private IInternalReporter? _reporter;
        private IInternalReporter? _fallback;

        /// <summary>
        /// Initializes a new instance of the CoreReporter class
        /// </summary>
        /// <param name="config">The configuration for the reporter</param>
        public CoreReporter(QaseConfig config)
        {
            _reporter = CreateReporter(config, config.Mode);
            _fallback = CreateReporter(config, config.Fallback);
            _logger = NullLogger<CoreReporter>.Instance;
        }

        /// <inheritdoc />
        public async Task startTestRun()
        {
            _logger.LogInformation("Starting test run");
            await ExecuteWithFallbackAsync(async () => await _reporter.startTestRun(), "start test run");
        }

        /// <inheritdoc />
        public async Task completeTestRun()
        {
            _logger.LogInformation("Completing test run");
            await ExecuteWithFallbackAsync(async () => await _reporter.completeTestRun(), "complete test run");
        }

        /// <inheritdoc />
        public async Task addResult(TestResult result)
        {
            _logger.LogDebug("Adding result: {Result}", result);
            await ExecuteWithFallbackAsync(async () => await _reporter.addResult(result), "add result");
        }

        /// <inheritdoc />
        public async Task uploadResults()
        {
            _logger.LogInformation("Uploading results");
            await ExecuteWithFallbackAsync(async () => await _reporter.uploadResults(), "upload results");
        }

        private async Task ExecuteWithFallbackAsync(Func<Task> action, string actionName)
        {
            if (_reporter != null)
            {
                try
                {
                    await action();
                }
                catch (QaseException ex)
                {
                    _logger.LogError(ex, "Failed to {ActionName} with reporter: {Message}", actionName, ex.Message);
                    await UseFallbackAsync();
                    await RetryActionAsync(action, actionName);
                }
            }
        }

        private async Task RetryActionAsync(Func<Task> action, string actionName)
        {
            if (_reporter != null)
            {
                try
                {
                    await action();
                }
                catch (QaseException ex)
                {
                    _logger.LogError(ex, "Failed to {ActionName} with reporter after fallback: {Message}", actionName, ex.Message);
                    _reporter = null;
                }
            }
        }

        private async Task UseFallbackAsync()
        {
            if (_fallback == null)
            {
                _reporter = null;
                return;
            }

            try
            {
                await _fallback.startTestRun();
                await _fallback.setResults(await _reporter.getResults());
                _reporter = _fallback;
                _fallback = null;
            }
            catch (QaseException ex)
            {
                _logger.LogError(ex, "Failed to start test run with fallback reporter");
                _reporter = null;
            }
        }

        private IInternalReporter? CreateReporter(QaseConfig config, Mode mode)
        {
            switch (mode)
            {
                case Mode.TestOps:
                    var client = new ApiClientV2(config);
                    return new TestopsReporter(config, client);
                case Mode.Report:
                    var writer = new FileWriter(config.Report.Connection.ToString());
                    return new FileReporter(config, writer);
                default:
                    return null;
            }
        }
    }
}
