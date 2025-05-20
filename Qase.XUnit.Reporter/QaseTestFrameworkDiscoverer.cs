using System;
using Xunit.Abstractions;

namespace Qase.XUnit.Reporter
{
    /// <summary>
    /// Custom test discoverer
    /// </summary>
    public class QaseTestFrameworkDiscoverer : ITestFrameworkDiscoverer, IDisposable
    {
        private readonly ITestFrameworkDiscoverer _discoverer;
        private readonly IMessageSink _loggerMessageSink;

        public QaseTestFrameworkDiscoverer(ITestFrameworkDiscoverer discoverer, IMessageSink loggerMessageSink)
        {
            _discoverer = discoverer;
            _loggerMessageSink = loggerMessageSink;
        }

        public string TargetFramework => _discoverer.TargetFramework;
        public string TestFrameworkDisplayName => _discoverer.TestFrameworkDisplayName;

        public void Find(bool includeSourceInformation, IMessageSink discoveryMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            var combinedSink = new MessageSinkWrapper(discoveryMessageSink, _loggerMessageSink);
            _discoverer.Find(includeSourceInformation, combinedSink, discoveryOptions);
        }

        public void Find(string typeName, bool includeSourceInformation, IMessageSink discoveryMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            var combinedSink = new MessageSinkWrapper(discoveryMessageSink, _loggerMessageSink);
            _discoverer.Find(typeName, includeSourceInformation, combinedSink, discoveryOptions);
        }

        public string Serialize(ITestCase testCase)
        {
            return _discoverer.Serialize(testCase);
        }

        public void Dispose()
        {
            (_discoverer as IDisposable)?.Dispose();
        }
    }
}
