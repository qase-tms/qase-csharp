using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Qase.Csharp.Commons.Reporters;
using Qase.Csharp.Commons.Models.Domain;
using Xunit.Abstractions;
using Serilog;
using Serilog.Events;
using Xunit;

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
        /// Converts ITestPassed to TestResult
        /// </summary>
        private TestResult ConvertToTestResult(ITestPassed testPassed)
        {
            var testResult = new TestResult
            {
                Title = testPassed.TestCase.DisplayName,
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Passed,
                    StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - (long)(testPassed.ExecutionTime * 1000),
                    EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    Duration = (int)(testPassed.ExecutionTime * 1000)
                },
                Signature = testPassed.TestCase.UniqueID
            };

            return testResult;
        }

        /// <summary>
        /// Converts ITestFailed to TestResult
        /// </summary>
        private TestResult ConvertToTestResult(ITestFailed testFailed)
        {
            var testResult = new TestResult
            {
                Title = testFailed.TestCase.DisplayName,
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Failed,
                    StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - (long)(testFailed.ExecutionTime * 1000),
                    EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    Duration = (int)(testFailed.ExecutionTime * 1000),
                    Stacktrace = testFailed.StackTraces.FirstOrDefault()
                },
                Message = testFailed.Messages.FirstOrDefault(),
                Signature = testFailed.TestCase.UniqueID
            };

            return testResult;
        }

        /// <summary>
        /// Converts ITestSkipped to TestResult
        /// </summary>
        private TestResult ConvertToTestResult(ITestSkipped testSkipped)
        {
            var testResult = new TestResult
            {
                Title = testSkipped.TestCase.DisplayName,
                Execution = new TestResultExecution
                {
                    Status = TestResultStatus.Skipped,
                    StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    EndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                },
                Message = testSkipped.Reason,
                Signature = testSkipped.TestCase.UniqueID
            };

            return testResult;
        }
    }
}
