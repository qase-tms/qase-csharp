using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Qase.Csharp.Commons.Reporters;
using Qase.Csharp.Commons.Models.Domain;
using Qase.Csharp.Commons.Models;
using Xunit.Abstractions;

namespace Qase.XUnit.Reporter
{
    /// <summary>
    /// Main listener implementation through IMessageSink
    /// </summary>
    public class QaseReporter : IMessageSink, IDisposable
    {
        private readonly IMessageSink _innerSink;
        private readonly ICoreReporter _reporter;
        private readonly TaskCompletionSource<bool> _completionSource;
        private bool _isDisposed;

        /// <summary>
        /// Creates a new instance of the listener
        /// </summary>
        /// <param name="innerSink">Inner message handler for further processing</param>
        public QaseReporter(IMessageSink innerSink)
        {
            _innerSink = innerSink;
            _reporter = CoreReporterFactory.GetInstance();
            _completionSource = new TaskCompletionSource<bool>();
        }

        /// <summary>
        /// Test framework message handler
        /// </summary>
        public bool OnMessage(IMessageSinkMessage message)
        {
            Console.WriteLine($"Processing message of type: {message.GetType().Name}");

            try
            {
                ProcessMessage(message)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                _completionSource.TrySetException(ex);
            }
            
            // Pass message to inner handler
            return _innerSink.OnMessage(message);
        }

        /// <summary>
        /// Process messages from test framework
        /// </summary>
        private async Task ProcessMessage(IMessageSinkMessage message)
        {
            if (message is ITestAssemblyStarting assemblyStarting)
            {
                await _reporter.startTestRun();
            }
            else if (message is ITestAssemblyFinished assemblyFinished)
            {
                try 
                {
                    await _reporter.uploadResults();
                    await _reporter.completeTestRun();
                    _completionSource.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    _completionSource.TrySetException(ex);
                    throw;
                }
            }
            else if (message is ITestPassed testPassed)
            {
                var testResult = ConvertToTestResult(testPassed);
                await _reporter.addResult(testResult);
            }
            else if (message is ITestFailed testFailed)
            {
                var testResult = ConvertToTestResult(testFailed);
                await _reporter.addResult(testResult);
            }
            else if (message is ITestSkipped testSkipped)
            {
                var testResult = ConvertToTestResult(testSkipped);
                await _reporter.addResult(testResult);
            }
        }

        /// <summary>
        /// Waits for all async operations to complete
        /// </summary>
        public Task WaitForCompletionAsync()
        {
            return _completionSource.Task;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                try
                {
                    // Ждем завершения всех операций при освобождении ресурсов
                    WaitForCompletionAsync()
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during cleanup: {ex}");
                }
                finally
                {
                    _isDisposed = true;
                }
            }
        }

        /// <summary>
        /// Generates a unique signature for a test case
        /// </summary>
        private string GenerateSignature(ITestCase testCase, System.Collections.Generic.Dictionary<string, string> parameters)
        {
            var method = testCase.TestMethod.Method;
            var declaringType = method.Type;
            
            // Get package name (namespace)
            var packageName = declaringType.Assembly.Name?.ToLower().Replace('.', ':') ?? "";
            
            // Get class name
            var className = declaringType.Name.ToLower();
            
            // Get method name
            var methodName = method.Name.ToLower();
            
            // // Get Qase IDs from attributes if present
            // var qaseIds = method.GetCustomAttributes("QaseId")
            //     .Select(attr => 
            //     {
            //         try 
            //         {
            //             var idProperty = attr.GetType().GetProperty("Id");
            //             return idProperty?.GetValue(attr)?.ToString();
            //         }
            //         catch
            //         {
            //             return null;
            //         }
            //     })
            //     .Where(id => id != null)
            //     .ToList();
            
            // var qaseIdPart = qaseIds.Any() 
            //     ? "::" + string.Join("-", qaseIds)
            //     : "";
            
            // Format parameters
            var parametersPart = parameters != null && parameters.Any()
                ? "::" + string.Join("::", parameters.Select(p => 
                    $"{p.Key.ToLower()}::{p.Value.ToLower().Replace(" ", "_")}"))
                : "";

            return $"{packageName}::{className}.cs::{className}::{methodName}{parametersPart}";
        }

        /// <summary>
        /// Creates base test result from test case
        /// </summary>
        private TestResult CreateBaseTestResult(ITestCase testCase, TestResultStatus status, long startTime, long endTime, int? duration = null, string message = null, string stacktrace = null)
        {
            var parameters = testCase.TestMethod.Method.GetParameters()
                .Zip(testCase.TestMethodArguments ?? Array.Empty<object>(), (parameter, value) => new
                {
                    parameter,
                    value
                })
                .ToDictionary(x => x.parameter.Name, x => x.value.ToString());

            return new TestResult
            {
                Title = testCase.TestMethod.Method.Name,
                Execution = new TestResultExecution
                {
                    Status = status,
                    StartTime = startTime,
                    EndTime = endTime,
                    Duration = duration,
                    Stacktrace = stacktrace
                },
                Message = message,
                Params = parameters,
                Signature = GenerateSignature(testCase, parameters),
                Relations = new Relations(){
                    Suite = new Suite()
                    {
                        Data = testCase.TestMethod.TestClass.Class.Name
                            .Split('.')
                            .Select(part => new SuiteData { Title = part })
                            .ToList()
                    }
                }
            };
        }

        /// <summary>
        /// Converts ITestPassed to TestResult
        /// </summary>
        private TestResult ConvertToTestResult(ITestPassed testPassed)
        {
            var endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var startTime = endTime - (long)(testPassed.ExecutionTime * 1000);
            var duration = (int)(testPassed.ExecutionTime * 1000);

            return CreateBaseTestResult(
                testPassed.TestCase,
                TestResultStatus.Passed,
                startTime,
                endTime,
                duration
            );
        }

        /// <summary>
        /// Converts ITestFailed to TestResult
        /// </summary>
        private TestResult ConvertToTestResult(ITestFailed testFailed)
        {
            var endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var startTime = endTime - (long)(testFailed.ExecutionTime * 1000);
            var duration = (int)(testFailed.ExecutionTime * 1000);

            return CreateBaseTestResult(
                testFailed.TestCase,
                TestResultStatus.Failed,
                startTime,
                endTime,
                duration,
                testFailed.Messages.FirstOrDefault(),
                testFailed.StackTraces.FirstOrDefault()
            );
        }

        /// <summary>
        /// Converts ITestSkipped to TestResult
        /// </summary>
        private TestResult ConvertToTestResult(ITestSkipped testSkipped)
        {
            var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            return CreateBaseTestResult(
                testSkipped.TestCase,
                TestResultStatus.Skipped,
                time,
                time,
                message: testSkipped.Reason
            );
        }
    }
}
