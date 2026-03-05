using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Testing.Platform.Extensions;
using Microsoft.Testing.Platform.Extensions.Messages;
using Microsoft.Testing.Platform.Extensions.TestHost;
using Microsoft.Testing.Platform.Services;

namespace Qase.XUnit.V3.Reporter
{
    public class QaseXUnitV3Extension : IDataConsumer, ITestSessionLifetimeHandler
    {
        public string Uid => "qase-xunit-v3-reporter";
        public string Version => "1.0.0";
        public string DisplayName => "Qase xUnit v3 Reporter";
        public string Description => "xUnit v3 integration with Qase TMS";
        public Type[] DataTypesConsumed => new[] { typeof(TestNodeUpdateMessage) };
        public Task<bool> IsEnabledAsync() => Task.FromResult(false);
        public Task ConsumeAsync(IDataProducer dataProducer, IData value, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task OnTestSessionStartingAsync(ITestSessionContext testSessionContext) => Task.CompletedTask;
        public Task OnTestSessionFinishingAsync(ITestSessionContext testSessionContext) => Task.CompletedTask;
    }
}
