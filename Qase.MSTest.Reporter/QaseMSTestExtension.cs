using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Testing.Platform.Extensions;
using Microsoft.Testing.Platform.Extensions.Messages;
using Microsoft.Testing.Platform.Extensions.TestHost;
using Microsoft.Testing.Platform.TestHost;
using Qase.Csharp.Commons;
using Qase.Csharp.Commons.Attributes;
using Qase.Csharp.Commons.Config;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Reporters;
using Qase.Csharp.Commons.Utils;

namespace Qase.MSTest.Reporter
{
    /// <summary>
    /// Main Qase TMS integration point for MSTest via the Microsoft Testing Platform (MTP v2).
    /// Implements <see cref="IDataConsumer"/> to receive test result messages and
    /// <see cref="ITestSessionLifetimeHandler"/> to hook into session start/finish lifecycle.
    ///
    /// Lifecycle flow:
    /// 1. Constructor loads QaseConfig to determine if reporting is enabled.
    /// 2. IsEnabledAsync returns false when Mode is Off, disabling the extension entirely.
    /// 3. OnTestSessionStartingAsync obtains the CoreReporter singleton and starts the test run.
    /// 4. OnTestSessionFinishingAsync uploads results and completes the test run.
    /// </summary>
    public class QaseMSTestExtension : IDataConsumer, ITestSessionLifetimeHandler
    {
        private ICoreReporter? _reporter;
        private readonly QaseConfig _config;

        /// <summary>
        /// Initializes a new instance of QaseMSTestExtension.
        /// Loads configuration eagerly so IsEnabledAsync can check Mode synchronously.
        /// </summary>
        public QaseMSTestExtension()
        {
            _config = ConfigFactory.LoadConfig();
        }

        /// <inheritdoc />
        public string Uid => "qase-mstest-reporter";

        /// <inheritdoc />
        public string Version => "1.0.0";

        /// <inheritdoc />
        public string DisplayName => "Qase MSTest Reporter";

        /// <inheritdoc />
        public string Description => "MSTest integration with Qase TMS";

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
                // Cast and validate the IData value
                if (value is not TestNodeUpdateMessage testNodeUpdateMessage)
                {
                    return;
                }

                var testNode = testNodeUpdateMessage.TestNode;

                // Extract state property and determine status
                var stateProperty = testNode.Properties.SingleOrDefault<TestNodeStateProperty>();
                if (stateProperty is null
                    or InProgressTestNodeStateProperty
                    or DiscoveredTestNodeStateProperty)
                {
                    return;
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
                        return;
                }

                // Create TestResult and set basic fields
                var testResult = new TestResult
                {
                    Title = testNode.DisplayName,
                    Execution = new TestResultExecution
                    {
                        Status = status,
                        Thread = Thread.CurrentThread.Name ?? Thread.CurrentThread.ManagedThreadId.ToString()
                    }
                };

                // Extract timing from TimingProperty
                var timingProperty = testNode.Properties.SingleOrDefault<TimingProperty>();
                if (timingProperty is not null)
                {
                    testResult.Execution.StartTime = timingProperty.GlobalTiming.StartTime.ToUnixTimeMilliseconds();
                    testResult.Execution.EndTime = timingProperty.GlobalTiming.EndTime.ToUnixTimeMilliseconds();
                    testResult.Execution.Duration = (int)timingProperty.GlobalTiming.Duration.TotalMilliseconds;
                }

                // Extract error details
                switch (stateProperty)
                {
                    case FailedTestNodeStateProperty failedState:
                        if (failedState.Exception is not null)
                        {
                            testResult.Message = failedState.Exception.Message;
                            testResult.Execution.Stacktrace = failedState.Exception.StackTrace;
                        }
                        else if (failedState.Explanation is not null)
                        {
                            testResult.Message = failedState.Explanation;
                        }
                        break;
                    case ErrorTestNodeStateProperty errorState:
                        if (errorState.Exception is not null)
                        {
                            testResult.Message = errorState.Exception.Message;
                            testResult.Execution.Stacktrace = errorState.Exception.StackTrace;
                        }
                        else if (errorState.Explanation is not null)
                        {
                            testResult.Message = errorState.Explanation;
                        }
                        break;
                    case SkippedTestNodeStateProperty skippedState:
                        if (skippedState.Explanation is not null)
                        {
                            testResult.Message = skippedState.Explanation;
                        }
                        break;
                }

                // Extract method identification from TestMethodIdentifierProperty (native MTP v2)
                // or VSTestProperty (VSTest bridge fallback)
                string? fullTypeName = null;
                string? methodName = null;
                string[] parameterTypeFullNames = Array.Empty<string>();

                var methodIdProperty = testNode.Properties.SingleOrDefault<TestMethodIdentifierProperty>();
                if (methodIdProperty is not null)
                {
                    fullTypeName = string.IsNullOrEmpty(methodIdProperty.Namespace)
                        ? methodIdProperty.TypeName
                        : $"{methodIdProperty.Namespace}.{methodIdProperty.TypeName}";
                    methodName = methodIdProperty.MethodName;
                    parameterTypeFullNames = methodIdProperty.ParameterTypeFullNames;
                }
                else
                {
                    // Fallback: extract from VSTestProperty (when running through VSTest bridge)
                    (fullTypeName, methodName, parameterTypeFullNames) = ExtractFromVSTestProperties(testNode);
                }

                if (fullTypeName is not null && methodName is not null)
                {
                    // Set default suite hierarchy from namespace + class name
                    testResult.Relations = new Relations
                    {
                        Suite = new Suite
                        {
                            Data = SuiteParser.FromTypeName(fullTypeName)
                        }
                    };

                    // Resolve Type and MethodInfo for attribute extraction
                    var type = TypeMethodResolver.ResolveType(fullTypeName);

                    if (type is not null)
                    {
                        var method = TypeMethodResolver.ResolveMethod(type, methodName, parameterTypeFullNames);

                        if (method is not null)
                        {
                            // Extract class-level and method-level Qase attributes
                            var classAttributes = type.GetCustomAttributes(typeof(IQaseAttribute), false).Cast<Attribute>();
                            var methodAttributes = method.GetCustomAttributes(typeof(IQaseAttribute), false).Cast<Attribute>();
                            AttributeExtractor.Apply(classAttributes, methodAttributes, testResult);

                            // Extract parameters from DisplayName for parameterized tests
                            var parsedParams = ParameterParser.ParseAndMap(testNode.DisplayName, method);
                            foreach (var kvp in parsedParams)
                            {
                                testResult.Params[kvp.Key] = kvp.Value;
                            }

                            // If no [Title] attribute override and parameters were found, use method name as Title
                            // (avoid showing "MethodName (val1,val2)" as Title -- params go in Params dict)
                            if (testResult.Title == testNode.DisplayName && testResult.Params.Count > 0)
                            {
                                testResult.Title = methodName;
                            }
                        }
                    }

                    // Generate display name matching QaseAspect format for ContextManager lookup
                    var contextDisplayName = $"{fullTypeName}.{methodName}";
                    if (testResult.Params.Count > 0)
                    {
                        var parameterStrings = testResult.Params.Select(kvp => $"{kvp.Key}: {kvp.Value}");
                        contextDisplayName += $"({string.Join(", ", parameterStrings)})";
                    }

                    // Retrieve steps, comments, and attachments from ContextManager
                    testResult.Steps = ContextManager.GetCompletedSteps(contextDisplayName);
                    var comments = ContextManager.GetComments(contextDisplayName);
                    if (!string.IsNullOrEmpty(comments))
                    {
                        testResult.Message = string.IsNullOrEmpty(testResult.Message)
                            ? comments
                            : string.Join("\n", testResult.Message, comments);
                    }
                    testResult.Attachments = ContextManager.GetAttachments(contextDisplayName);
                }

                // Generate test signature for cross-run correlation (must always be set — API v2 requires it)
                testResult.Signature = Signature.Generate(
                    testResult.TestopsIds,
                    testResult.Relations?.Suite?.Data?.Select(suite => suite.Title),
                    testResult.Params);

                if (string.IsNullOrEmpty(testResult.Signature))
                {
                    testResult.Signature = testResult.Title?.ToLower().Trim().Replace(" ", "-") ?? "unknown";
                }

                // Submit result via CoreReporter (native async), skip when Ignore is set
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
        /// Extracts method identification from VSTestProperty instances on the TestNode.
        /// Used as fallback when TestMethodIdentifierProperty is not available (VSTest bridge mode).
        /// Reads TestCase.ManagedType and TestCase.ManagedMethod via reflection to avoid
        /// a hard dependency on the Microsoft.Testing.Extensions.VSTestBridge package.
        /// </summary>
        private static (string? fullTypeName, string? methodName, string[] parameterTypeFullNames) ExtractFromVSTestProperties(TestNode testNode)
        {
            string? managedType = null;
            string? managedMethod = null;

            // Materialize properties into a list to avoid enumerator exhaustion issues
            var allProperties = testNode.Properties.AsEnumerable().ToList();
            foreach (var prop in allProperties)
            {
                var propTypeName = prop.GetType().FullName;
                if (propTypeName != "Microsoft.Testing.Extensions.VSTestBridge.ObjectModel.VSTestProperty")
                    continue;

                var testPropertyObj = prop.GetType().GetProperty("Property")?.GetValue(prop);
                var testCaseObj = prop.GetType().GetProperty("TestCase")?.GetValue(prop);
                if (testPropertyObj is null || testCaseObj is null)
                    continue;

                var id = testPropertyObj.GetType().GetProperty("Id")?.GetValue(testPropertyObj) as string;
                if (id is null) continue;

                if (id != "TestCase.ManagedType" && id != "TestCase.ManagedMethod")
                    continue;

                // Call TestCase.GetPropertyValue(TestProperty) to retrieve the value
                var getValueMethod = testCaseObj.GetType().GetMethod(
                    "GetPropertyValue",
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    new[] { testPropertyObj.GetType() },
                    null);
                var value = getValueMethod?.Invoke(testCaseObj, new[] { testPropertyObj }) as string;

                switch (id)
                {
                    case "TestCase.ManagedType":
                        managedType = value;
                        break;
                    case "TestCase.ManagedMethod":
                        managedMethod = value;
                        break;
                }

                if (managedType is not null && managedMethod is not null) break;
            }

            if (managedType is null || managedMethod is null)
                return (null, null, Array.Empty<string>());

            // Parse ManagedMethod: "MethodName(System.String,System.Int32)" or "MethodName"
            string methodName;
            string[] parameterTypeFullNames;
            var parenIdx = managedMethod.IndexOf('(');
            if (parenIdx > 0 && managedMethod.EndsWith(")"))
            {
                methodName = managedMethod.Substring(0, parenIdx);
                var paramStr = managedMethod.Substring(parenIdx + 1, managedMethod.Length - parenIdx - 2);
                parameterTypeFullNames = string.IsNullOrEmpty(paramStr)
                    ? Array.Empty<string>()
                    : paramStr.Split(',').Select(s => s.Trim()).ToArray();
            }
            else
            {
                methodName = managedMethod;
                parameterTypeFullNames = Array.Empty<string>();
            }

            return (managedType, methodName, parameterTypeFullNames);
        }

        /// <inheritdoc />
        public async Task OnTestSessionStartingAsync(SessionUid sessionUid, CancellationToken cancellationToken)
        {
            _reporter = CoreReporterFactory.GetInstance();
            await _reporter.startTestRun();
        }

        /// <inheritdoc />
        public async Task OnTestSessionFinishingAsync(SessionUid sessionUid, CancellationToken cancellationToken)
        {
            if (_reporter != null)
            {
                await _reporter.uploadResults();
                await _reporter.completeTestRun();
            }
        }
    }
}
