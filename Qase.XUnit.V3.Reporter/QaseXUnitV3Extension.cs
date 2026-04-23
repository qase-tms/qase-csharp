using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Testing.Platform.Extensions;
using Microsoft.Testing.Platform.Extensions.Messages;
using Microsoft.Testing.Platform.Extensions.TestHost;
using Microsoft.Testing.Platform.Services;
using Qase.Csharp.Commons;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Qase.Csharp.Commons.Utils;

namespace Qase.XUnit.V3.Reporter
{
    /// <summary>
    /// Main Qase TMS integration point for xUnit v3 via the Microsoft Testing Platform (MTP v2).
    /// Implements <see cref="IDataConsumer"/> to receive test result messages and
    /// <see cref="ITestSessionLifetimeHandler"/> to hook into session start/finish lifecycle.
    ///
    /// Lifecycle flow:
    /// 1. Constructor loads QaseConfig to determine if reporting is enabled.
    /// 2. IsEnabledAsync returns false when Mode is Off, disabling the extension entirely.
    /// 3. OnTestSessionStartingAsync obtains the CoreReporter singleton and starts the test run.
    /// 4. OnTestSessionFinishingAsync uploads results and completes the test run.
    ///
    /// Key difference from MSTest reporter: xUnit v3 natively populates TestMethodIdentifierProperty
    /// on TestNode when running via MTP -- no VSTest bridge fallback is needed.
    /// </summary>
    public class QaseXUnitV3Extension : IDataConsumer, ITestSessionLifetimeHandler
    {
        private ICoreReporter? _reporter;
        private readonly QaseConfig _config;
        private readonly ITestResultBuilder _builder = new TestResultBuilder();

        /// <summary>
        /// Initializes a new instance of QaseXUnitV3Extension.
        /// Loads configuration eagerly so IsEnabledAsync can check Mode synchronously.
        /// </summary>
        public QaseXUnitV3Extension()
        {
            _config = ConfigFactory.LoadConfig();
        }

        /// <inheritdoc />
        public string Uid => "qase-xunit-v3-reporter";

        /// <inheritdoc />
        public string Version => "1.0.0";

        /// <inheritdoc />
        public string DisplayName => "Qase xUnit v3 Reporter";

        /// <inheritdoc />
        public string Description => "xUnit v3 integration with Qase TMS";

        /// <inheritdoc />
        public Type[] DataTypesConsumed => new[] { typeof(TestNodeUpdateMessage) };

        /// <inheritdoc />
        public Task<bool> IsEnabledAsync()
        {
            return Task.FromResult(_config.Mode != Mode.Off);
        }

        /// <inheritdoc />
        public async Task ConsumeAsync(IDataProducer dataProducer, IData value, CancellationToken cancellationToken)
        {
            try
            {
                if (value is not TestNodeUpdateMessage testNodeUpdateMessage)
                    return;

                var testNode = testNodeUpdateMessage.TestNode;
                var raw = MapToRawTestData(testNode);
                if (raw == null)
                    return;

                var testResult = _builder.Build(raw);

                if (_reporter != null && !testResult.Ignore)
                {
                    await _reporter.addResult(testResult);
                }
            }
            catch (Exception ex)
            {
                // Catch and log to prevent the testing platform from disabling this extension
                // after an unhandled exception in ConsumeAsync
                Console.Error.WriteLine($"[Qase] ConsumeAsync error: {ex.GetType().Name}: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps a TestNode to RawTestData, extracting status, timing, error details, and method ID.
        /// Returns null when the node state should not produce a result (in-progress, discovered, etc.).
        /// </summary>
        private static RawTestData? MapToRawTestData(TestNode testNode)
        {
            var stateProperty = testNode.Properties.SingleOrDefault<TestNodeStateProperty>();
            if (stateProperty is null
                or InProgressTestNodeStateProperty
                or DiscoveredTestNodeStateProperty)
            {
                return null;
            }

            TestResultStatus status;
            switch (stateProperty)
            {
                case PassedTestNodeStateProperty:
                    status = TestResultStatus.Passed;
                    break;
                case FailedTestNodeStateProperty:
                    status = TestResultStatus.Failed;
                    break;
                case ErrorTestNodeStateProperty:
                    status = TestResultStatus.Invalid;
                    break;
                case SkippedTestNodeStateProperty:
                    status = TestResultStatus.Skipped;
                    break;
                default:
                    return null;
            }

            var raw = new RawTestData
            {
                DisplayName = testNode.DisplayName,
                Status = status,
                Thread = Thread.CurrentThread.Name ?? Thread.CurrentThread.ManagedThreadId.ToString()
            };

            // Extract timing from TimingProperty
            var timingProperty = testNode.Properties.SingleOrDefault<TimingProperty>();
            if (timingProperty is not null)
            {
                raw.StartTime = timingProperty.GlobalTiming.StartTime.ToUnixTimeMilliseconds();
                raw.EndTime = timingProperty.GlobalTiming.EndTime.ToUnixTimeMilliseconds();
                raw.Duration = (int)timingProperty.GlobalTiming.Duration.TotalMilliseconds;
            }

            // Extract error details
            switch (stateProperty)
            {
                case FailedTestNodeStateProperty failedState:
                    if (failedState.Exception is not null)
                    {
                        raw.ErrorMessage = failedState.Exception.Message;
                        raw.StackTrace = failedState.Exception.StackTrace;
                    }
                    else if (failedState.Explanation is not null)
                    {
                        raw.ErrorMessage = failedState.Explanation;
                    }
                    break;
                case ErrorTestNodeStateProperty errorState:
                    if (errorState.Exception is not null)
                    {
                        raw.ErrorMessage = errorState.Exception.Message;
                        raw.StackTrace = errorState.Exception.StackTrace;
                    }
                    else if (errorState.Explanation is not null)
                    {
                        raw.ErrorMessage = errorState.Explanation;
                    }
                    break;
                case SkippedTestNodeStateProperty skippedState:
                    if (skippedState.Explanation is not null)
                    {
                        raw.ErrorMessage = skippedState.Explanation;
                    }
                    break;
            }

            // Extract method identification from TestMethodIdentifierProperty (native MTP v2).
            // xUnit v3 natively populates this property -- no VSTest bridge fallback needed.
            var methodIdProperty = testNode.Properties.SingleOrDefault<TestMethodIdentifierProperty>();
            if (methodIdProperty is not null)
            {
                raw.FullTypeName = string.IsNullOrEmpty(methodIdProperty.Namespace)
                    ? methodIdProperty.TypeName
                    : $"{methodIdProperty.Namespace}.{methodIdProperty.TypeName}";
                raw.MethodName = methodIdProperty.MethodName;
                raw.ParameterTypeFullNames = methodIdProperty.ParameterTypeFullNames;
            }
            else
            {
                Console.Error.WriteLine(
                    $"[Qase] TestMethodIdentifierProperty not found for test: {testNode.DisplayName}");
            }

            return raw;
        }

        /// <inheritdoc />
        public async Task OnTestSessionStartingAsync(ITestSessionContext testSessionContext)
        {
            _reporter = CoreReporterFactory.GetInstance();
            await _reporter.startTestRun();
        }

        /// <inheritdoc />
        public async Task OnTestSessionFinishingAsync(ITestSessionContext testSessionContext)
        {
            if (_reporter != null)
            {
                await _reporter.uploadResults();
                await _reporter.completeTestRun();
            }
        }
    }
}
