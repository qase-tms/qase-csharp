using Xunit.Abstractions;

namespace Qase.XUnit.Reporter
{
    /// <summary>
    /// Wrapper for combining two IMessageSink instances
    /// </summary>
    public class MessageSinkWrapper : IMessageSink
    {
        private readonly IMessageSink _primarySink;
        private readonly IMessageSink _secondarySink;

        public MessageSinkWrapper(IMessageSink primarySink, IMessageSink secondarySink)
        {
            _primarySink = primarySink;
            _secondarySink = secondarySink;
        }

        public bool OnMessage(IMessageSinkMessage message)
        {
            var primaryResult = _primarySink.OnMessage(message);
            var secondaryResult = _secondarySink.OnMessage(message);
            return primaryResult && secondaryResult;
        }
    }
}
