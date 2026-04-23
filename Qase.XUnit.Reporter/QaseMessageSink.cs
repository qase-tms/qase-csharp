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
        private readonly ICoreReporter _reporter;
        private ITestResultBuilder _builder = new TestResultBuilder();

        private readonly ConcurrentDictionary<ITest, RawTestData> qaseTestData = new();

        public QaseMessageSink(IRunnerLogger logger) : base(logger)
        {
            this.Runner.TestAssemblyExecutionStartingEvent +=
                this.OnTestAssemblyExecutionStarting;
            this.Runner.TestAssemblyExecutionFinishedEvent +=
                this.OnTestAssemblyExecutionFinished;

            this.Execution.TestStartingEvent += this.OnTestStarting;
            this.Execution.TestFailedEvent += this.OnTestFailed;
            this.Execution.TestPassedEvent += this.OnTestPassed;
            this.Execution.TestSkippedEvent += this.OnTestSkipped;
            this.Execution.TestFinishedEvent += this.OnTestFinished;

            CurrentSink ??= this;
            _reporter = CoreReporterFactory.GetInstance();
        }

        private void OnTestAssemblyExecutionStarting(MessageHandlerArgs<ITestAssemblyExecutionStarting> args)
        {
            try
            {
                _reporter.startTestRun().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                this.Logger.LogWarning($"Failed to start test run: {ex}");
            }
        }

        private void OnTestStarting(MessageHandlerArgs<ITestStarting> args)
        {
            var testCase = args.Message.Test.TestCase;

            // Parse parameters eagerly while we have full ITestCase context
            var parameters = testCase.TestMethod.Method.GetParameters()
                .Zip(testCase.TestMethodArguments ?? Array.Empty<object>(), (parameter, value) => new { parameter, value })
                .ToDictionary(x => x.parameter.Name, x =>
                {
                    if (x.value is null) return "null";
                    if (!string.IsNullOrWhiteSpace(x.value?.ToString())) return x.value.ToString();
                    var size = x.value?.ToString().Length ?? 0;
                    return size == 0 ? "empty" : $"empty ({size})";
                });

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

            qaseTestData[args.Message.Test] = raw;
        }

        private void OnTestPassed(MessageHandlerArgs<ITestPassed> args)
        {
            var raw = qaseTestData[args.Message.Test];
            raw.Status = TestResultStatus.Passed;
            raw.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            raw.Duration = (int)(args.Message.ExecutionTime * 1000);
        }

        private void OnTestFailed(MessageHandlerArgs<ITestFailed> args)
        {
            var raw = qaseTestData[args.Message.Test];
            raw.Status = TestResultStatus.Failed;
            raw.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            raw.Duration = (int)(args.Message.ExecutionTime * 1000);
            raw.ErrorMessage = string.Join("\n", args.Message.Messages);
            raw.StackTrace = string.Join("\n", args.Message.StackTraces);
            raw.ExceptionTypes = args.Message.ExceptionTypes;
        }

        private void OnTestSkipped(MessageHandlerArgs<ITestSkipped> args)
        {
            var raw = qaseTestData[args.Message.Test];
            raw.Status = TestResultStatus.Skipped;
            raw.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            raw.Duration = (int)(args.Message.ExecutionTime * 1000);
            raw.ErrorMessage = args.Message.Reason;
        }

        private void OnTestFinished(MessageHandlerArgs<ITestFinished> args)
        {
            var raw = qaseTestData[args.Message.Test];
            var testResult = _builder.Build(raw);

            if (!testResult.Ignore)
            {
                _reporter.addResult(testResult).GetAwaiter().GetResult();
            }

            qaseTestData.TryRemove(args.Message.Test, out _);
        }

        private void OnTestAssemblyExecutionFinished(MessageHandlerArgs<ITestAssemblyExecutionFinished> args)
        {
            try
            {
                _reporter.uploadResults().GetAwaiter().GetResult();
                _reporter.completeTestRun().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                this.Logger.LogWarning($"Error in OnTestAssemblyExecutionFinished: {ex}");
            }
        }
    }
}
