using System;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Qase.XUnit.Reporter
{
    /// <summary>
    /// Custom test executor
    /// </summary>
    public class QaseTestFrameworkExecutor : ITestFrameworkExecutor
    {
        private readonly ITestFrameworkExecutor _executor;
        private readonly QaseReporter _loggerMessageSink;

        public QaseTestFrameworkExecutor(ITestFrameworkExecutor executor, QaseReporter loggerMessageSink)
        {
            _executor = executor;
            _loggerMessageSink = loggerMessageSink;
        }

        public void Dispose()
        {
            try
            {
                _loggerMessageSink.WaitForCompletionAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error waiting for logger completion: {ex}");
            }
            finally
            {
                _executor.Dispose();
            }
        }

        public ITestCase Deserialize(string value)
        {
            return _executor.Deserialize(value);
        }

        public void RunAll(IMessageSink executionMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions, ITestFrameworkExecutionOptions executionOptions)
        {
            var combinedSink = new MessageSinkWrapper(executionMessageSink, _loggerMessageSink);
            _executor.RunAll(combinedSink, discoveryOptions, executionOptions);
        }

        public void RunTests(IEnumerable<ITestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            var combinedSink = new MessageSinkWrapper(executionMessageSink, _loggerMessageSink);
            _executor.RunTests(testCases, combinedSink, executionOptions);
        }
    }
}
