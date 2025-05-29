using Xunit;
using Xunit.Abstractions;

namespace Qase.Xunit.Reporter
{
    public class QaseRunnerReporter : IRunnerReporter
    {
        public string Description { get; }
            = "xUnit Integration with Qase TMS";

        public bool IsEnvironmentallyEnabled { get; } = true;

        public string RunnerSwitch { get; } = "qase";

        public IMessageSink CreateMessageHandler(IRunnerLogger logger)
        {
            return new QaseMessageSink(logger);
        }
    }
}
