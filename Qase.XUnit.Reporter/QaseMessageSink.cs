using System;
using System.Collections.Concurrent;
using System.Linq;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Xunit;
using Xunit.Abstractions;
using Qase.Csharp.Commons.Utils;
using Qase.Csharp.Commons;

namespace Qase.Xunit.Reporter
{
    internal class QaseMessageSink : DefaultRunnerReporterWithTypesMessageHandler
    {
        internal static QaseMessageSink? CurrentSink { get; private set; }
        private readonly ICoreReporter? _reporter;
        private ITestResultBuilder _builder = new TestResultBuilder();

        private readonly ConcurrentDictionary<ITest, RawTestData> qaseTestData = new();

        public QaseMessageSink(IRunnerLogger logger) : base(logger)
        {
            CurrentSink ??= this;

            // Off mode: stay unsubscribed so no reporter is built and no Qase
            // output reaches the test run.
            if (!CoreReporterFactory.IsReportingEnabled())
            {
                return;
            }

            this.Runner.TestAssemblyExecutionStartingEvent +=
                this.OnTestAssemblyExecutionStarting;
            this.Runner.TestAssemblyExecutionFinishedEvent +=
                this.OnTestAssemblyExecutionFinished;

            this.Execution.TestStartingEvent += this.OnTestStarting;
            this.Execution.TestFailedEvent += this.OnTestFailed;
            this.Execution.TestPassedEvent += this.OnTestPassed;
            this.Execution.TestSkippedEvent += this.OnTestSkipped;
            this.Execution.TestFinishedEvent += this.OnTestFinished;

            _reporter = CoreReporterFactory.GetInstance();
        }

        private void OnTestAssemblyExecutionStarting(MessageHandlerArgs<ITestAssemblyExecutionStarting> args)
        {
            try
            {
                _reporter!.startTestRun().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                this.Logger.LogWarning($"Failed to start test run: {ex}");
            }
        }

        private void OnTestStarting(MessageHandlerArgs<ITestStarting> args)
        {
            try
            {
                HandleTestStarting(args.Message);
            }
            catch (Exception ex)
            {
                this.Logger.LogWarning($"[Qase] Failed to handle test start: {ex.Message}");
            }
        }

        private void HandleTestStarting(ITestStarting message)
        {
            var testCase = message.Test.TestCase;

            // Same formatting as QaseAspect, so the context key built from these values matches.
            var parameters = testCase.TestMethod.Method.GetParameters()
                .Zip(testCase.TestMethodArguments ?? Array.Empty<object>(), (parameter, value) => new { parameter, value })
                .ToDictionary(x => x.parameter.Name, x => DisplayNameGenerator.FormatArgument(x.value));

            var raw = new RawTestData
            {
                DisplayName = testCase.TestMethod.Method.Name,
                Status = TestResultStatus.Skipped,
                Thread = System.Threading.Thread.CurrentThread.Name ??
                    System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(),
                FullTypeName = testCase.TestMethod.TestClass.Class.Name,
                MethodName = testCase.TestMethod.Method.Name,
                PreParsedParams = parameters.Count > 0 ? parameters : null,
                StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            qaseTestData[message.Test] = raw;
        }

        private void OnTestPassed(MessageHandlerArgs<ITestPassed> args)
        {
            if (!qaseTestData.TryGetValue(args.Message.Test, out var raw))
                return;
            raw.Status = TestResultStatus.Passed;
            raw.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            raw.Duration = (int)(args.Message.ExecutionTime * 1000);
        }

        private void OnTestFailed(MessageHandlerArgs<ITestFailed> args)
        {
            if (!qaseTestData.TryGetValue(args.Message.Test, out var raw))
                return;
            raw.Status = TestResultStatus.Failed;
            raw.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            raw.Duration = (int)(args.Message.ExecutionTime * 1000);
            raw.ErrorMessage = string.Join("\n", args.Message.Messages);
            raw.StackTrace = string.Join("\n", args.Message.StackTraces);
            raw.ExceptionTypes = args.Message.ExceptionTypes;
        }

        private void OnTestSkipped(MessageHandlerArgs<ITestSkipped> args)
        {
            if (!qaseTestData.TryGetValue(args.Message.Test, out var raw))
                return;
            raw.Status = TestResultStatus.Skipped;
            raw.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            raw.Duration = (int)(args.Message.ExecutionTime * 1000);
            raw.ErrorMessage = args.Message.Reason;
        }

        private void OnTestFinished(MessageHandlerArgs<ITestFinished> args)
        {
            if (!qaseTestData.TryRemove(args.Message.Test, out var raw))
                return;

            try
            {
                var testResult = _builder.Build(raw);

                if (!testResult.Ignore)
                {
                    _reporter!.addResult(testResult).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                // An exception here would reach xUnit's message bus and abort the run.
                this.Logger.LogWarning($"[Qase] Failed to report test result: {ex.Message}");
            }
        }

        private void OnTestAssemblyExecutionFinished(MessageHandlerArgs<ITestAssemblyExecutionFinished> args)
        {
            try
            {
                _reporter!.uploadResults().GetAwaiter().GetResult();
                _reporter!.completeTestRun().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                this.Logger.LogWarning($"Error in OnTestAssemblyExecutionFinished: {ex}");
            }
        }
    }
}
