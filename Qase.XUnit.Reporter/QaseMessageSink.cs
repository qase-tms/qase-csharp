using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Xunit;
using Xunit.Abstractions;

namespace Qase.Xunit.Reporter
{
    internal class QaseMessageSink : DefaultRunnerReporterWithTypesMessageHandler
    {
        internal static QaseMessageSink? CurrentSink { get; private set; }
        private readonly ICoreReporter _reporter;

        private readonly ConcurrentDictionary<ITest, TestResult> qaseTestData = new();

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
            qaseTestData[args.Message.Test] = CreateBaseTestResult(args.Message.Test.TestCase);
        }

        private void OnTestPassed(MessageHandlerArgs<ITestPassed> args)
        {
            var testResult = qaseTestData[args.Message.Test];
            testResult.Execution!.Status = TestResultStatus.Passed;
            testResult.Execution!.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            testResult.Execution!.Duration = (int)(args.Message.ExecutionTime * 1000);
        }

        private void OnTestFailed(MessageHandlerArgs<ITestFailed> args)
        {
            var testResult = qaseTestData[args.Message.Test];
            testResult.Execution!.Status = TestResultStatus.Failed;
            testResult.Execution!.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            testResult.Execution!.Duration = (int)(args.Message.ExecutionTime * 1000);
            testResult.Message = string.Join("\n", args.Message.Messages);
            testResult.Execution.Stacktrace = string.Join("\n", args.Message.StackTraces);
        }

        private void OnTestSkipped(MessageHandlerArgs<ITestSkipped> args)
        {
            var testResult = qaseTestData[args.Message.Test];
            testResult.Execution!.Status = TestResultStatus.Skipped;
            testResult.Execution!.EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            testResult.Execution!.Duration = (int)(args.Message.ExecutionTime * 1000);
            testResult.Message = args.Message.Reason;
        }

        private void OnTestFinished(MessageHandlerArgs<ITestFinished> args)
        {
            var testResult = qaseTestData[args.Message.Test];
            // testResult.Steps = ContextManager.GetCompletedSteps(args.Message.Test.TestCase.DisplayName);
            // testResult.Message = string.Join("\n", testResult.Message, ContextManager.GetComments(args.Message.Test.TestCase.DisplayName));
            // testResult.Attachments = ContextManager.GetAttachments(args.Message.Test.TestCase.DisplayName);
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

        private TestResult CreateBaseTestResult(ITestCase testCase)
        {
            if (testCase == null)
            {
                throw new ArgumentNullException(nameof(testCase));
            }

            var parameters = testCase.TestMethod.Method.GetParameters()
                .Zip(testCase.TestMethodArguments ?? Array.Empty<object>(), (parameter, value) => new
                {
                    parameter,
                    value
                })
                .ToDictionary(x => x.parameter.Name, x => x.value?.ToString() ?? "null");

            var result = new TestResult
            {
                Title = testCase.TestMethod.Method.Name,
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Skipped,
                    Thread = System.Threading.Thread.CurrentThread.Name ??
                        System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()
                },
                Message = null,
                Params = parameters,
                Signature = GenerateSignature(testCase, parameters),
                Relations = new Relations()
                {
                    Suite = new Suite()
                    {
                        Data = testCase.TestMethod.TestClass.Class.Name
                            .Split('.')
                            .Select(part => new SuiteData { Title = part })
                            .ToList()
                    }
                }
            };

            var attributes = testCase.TestMethod.Method.GetCustomAttributes(typeof(IQaseAttribute));
            var classAttributes = testCase.TestMethod.TestClass.Class.GetCustomAttributes(typeof(IQaseAttribute));

            foreach (var attribute in classAttributes.Concat(attributes))
            {
                switch (((IReflectionAttributeInfo)attribute).Attribute)
                {
                    case QaseIdsAttribute qaseIdsAttribute:
                        result.TestopsIds = qaseIdsAttribute.Ids;
                        break;
                    case TitleAttribute titleAttribute:
                        result.Title = titleAttribute.Title;
                        break;
                    case FieldsAttribute fieldsAttribute:
                        result.Fields.Add(fieldsAttribute.Key, fieldsAttribute.Value);
                        break;
                    case SuitesAttribute suitesAttribute:
                        result.Relations.Suite.Data = suitesAttribute.Suites.Select(suite => new SuiteData { Title = suite }).ToList();
                        break;
                    case IgnoreAttribute ignoreAttribute:
                        result.Ignore = true;
                        break;
                }
            }

            return result;
        }

        private string GenerateSignature(ITestCase testCase, Dictionary<string, string> parameters)
        {
            if (testCase == null)
            {
                throw new ArgumentNullException(nameof(testCase));
            }

            var method = testCase.TestMethod.Method;
            var declaringType = method.Type;

            // Get class name
            var className = declaringType.Name.ToLower().Replace(".", "::");

            // Get method name
            var methodName = method.Name.ToLower();

            // Get Qase IDs from attributes if present
            var qaseIds = method.GetCustomAttributes(typeof(IQaseAttribute))
                .Where(attr => attr is QaseIdsAttribute)
                .Select(attr => ((QaseIdsAttribute)attr).Ids)
                .FirstOrDefault() ?? new List<long>();

            var qaseIdPart = qaseIds.Any()
                ? "::" + string.Join("-", qaseIds)
                : "";

            // Format parameters
            var parametersPart = parameters != null && parameters.Any()
                ? "::" + string.Join("::", parameters.Select(p =>
                    $"{p.Key.ToLower()}::{p.Value?.ToLower().Replace(" ", "_")}"))
                : "";

            return $"{className}::{methodName}{qaseIdPart}{parametersPart}";
        }
    }
}
