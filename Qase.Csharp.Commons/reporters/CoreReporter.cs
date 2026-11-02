using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Core;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Utils;

namespace Qase.Csharp.Commons.Reporters
{
    /// <summary>
    /// Core reporter implementation that manages primary and fallback reporters
    /// </summary>
    public class CoreReporter : ICoreReporter
    {
        private readonly ILogger<CoreReporter> _logger;
        private readonly QaseConfig _config;
        private IInternalReporter? _reporter;
        private IInternalReporter? _fallback;
        private readonly SemaphoreSlim _gate = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the CoreReporter class
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="config">The configuration</param>
        /// <param name="reporter">The primary reporter</param>
        /// <param name="fallback">The fallback reporter</param>
        public CoreReporter(
            ILogger<CoreReporter> logger,
            QaseConfig config,
            IInternalReporter? reporter = null,
            IInternalReporter? fallback = null)
        {
            _logger = logger;
            _config = config;
            _reporter = reporter;
            _fallback = fallback;

            // Collect and log host information including all versions
            var hostInfo = HostInfo.GetHostInfo(null);
            _logger.LogDebug("Using host info: {HostInfo}", hostInfo.ToString());
        }

        /// <inheritdoc />
        public Task startTestRun()
        {
            _logger.LogInformation("Starting test run");
            return ExecuteWithFallbackAsync(() => _reporter!.startTestRun(), "start test run");
        }

        /// <inheritdoc />
        public Task completeTestRun()
        {
            _logger.LogInformation("Completing test run");
            return ExecuteWithFallbackAsync(() => _reporter!.completeTestRun(), "complete test run");
        }

        /// <inheritdoc />
        public Task addResult(TestResult result)
        {
            _logger.LogDebug("Adding result: {Result}", result);

            // Apply status mapping if configured
            if (_config?.StatusMapping?.Count > 0)
            {
                StatusMappingUtils.ApplyStatusMapping(result, _config.StatusMapping, _logger);
            }

            return ExecuteWithFallbackAsync(
                () => _reporter!.addResult(result),
                "add result",
                // A failed batch upload has already buffered this result, and the fallback took the buffer over.
                async () =>
                {
                    var buffered = await _reporter!.getResults().ConfigureAwait(false);
                    if (buffered == null || !buffered.Contains(result))
                    {
                        await _reporter.addResult(result).ConfigureAwait(false);
                    }
                });
        }

        /// <inheritdoc />
        public Task uploadResults()
        {
            _logger.LogInformation("Uploading results");
            return ExecuteWithFallbackAsync(() => _reporter!.uploadResults(), "upload results");
        }

        private async Task ExecuteWithFallbackAsync(Func<Task> action, string actionName, Func<Task>? retryAction = null)
        {
            // Framework hooks run concurrently (parallel scenarios) and block on these tasks
            // from test threads; one gate keeps the buffers consistent, and running off the
            // caller's SynchronizationContext keeps a blocking caller from deadlocking.
            await _gate.WaitAsync().ConfigureAwait(false);
            try
            {
                await Task.Run(async () =>
                {
                    if (_reporter == null)
                        return;

                    try
                    {
                        await action().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to {ActionName} with reporter: {Message}", actionName, ex.Message);
                        await UseFallbackAsync().ConfigureAwait(false);
                        await RetryActionAsync(retryAction ?? action, actionName).ConfigureAwait(false);
                    }
                }).ConfigureAwait(false);
            }
            finally
            {
                _gate.Release();
            }
        }

        private async Task RetryActionAsync(Func<Task> action, string actionName)
        {
            if (_reporter != null)
            {
                try
                {
                    await action().ConfigureAwait(false);
                }
                catch (Exception ex)
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
                await _fallback.startTestRun().ConfigureAwait(false);
                await _fallback.setResults(await _reporter!.getResults().ConfigureAwait(false)).ConfigureAwait(false);
                _reporter = _fallback;
                _fallback = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start test run with fallback reporter");
                _reporter = null;
            }
        }
    }
}
