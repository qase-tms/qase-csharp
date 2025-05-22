using System;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Qase.XUnit.Reporter
{
    /// <summary>
    /// Custom xUnit test framework that automatically connects the listener
    /// </summary>
    public class QaseXunitTestFramework : ITestFramework, IDisposable
    {
        private readonly IMessageSink _originalMessageSink;
        private readonly QaseReporter _loggerMessageSink;
        private readonly XunitTestFramework _framework;

        /// <summary>
        /// Creates a new instance of the custom framework
        /// </summary>
        public QaseXunitTestFramework(IMessageSink messageSink)
        {
            _originalMessageSink = messageSink;
            _loggerMessageSink = new QaseReporter(messageSink);
            _framework = new XunitTestFramework(_originalMessageSink);
        }

        public ISourceInformationProvider SourceInformationProvider
        {
            get => _framework.SourceInformationProvider;
            set => _framework.SourceInformationProvider = value;
        }

        public ITestFrameworkDiscoverer GetDiscoverer(IAssemblyInfo assembly)
        {
            var discoverer = _framework.GetDiscoverer(assembly);
            return new QaseTestFrameworkDiscoverer(discoverer, _loggerMessageSink);
        }

        public ITestFrameworkExecutor GetExecutor(AssemblyName assemblyName)
        {
            var executor = _framework.GetExecutor(assemblyName);
            return new QaseTestFrameworkExecutor(executor, _loggerMessageSink);
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
                _framework.Dispose();
            }
        }
    }
}
