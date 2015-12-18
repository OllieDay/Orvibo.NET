using System.Linq;

namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Acts as the base class for message containing state data.
    /// </summary>
    internal abstract class InboundStateMessage : InboundMessage
    {
        /// <summary>
        ///     Initializes a new instance of the InboundStateMessage class.
        /// </summary>
        /// <param name="data">Message data.</param>
        protected InboundStateMessage(byte[] data) : base(data)
        {
            ParseState();
        }

        /// <summary>
        ///     Gets the socket state.
        /// </summary>
        public SocketState State { get; private set; }

        /// <summary>
        ///     Parses the message data for the socket state.
        /// </summary>
        private void ParseState()
        {
            var state = Data.Last();

            switch (state)
            {
                case 0x00:
                    State = SocketState.Off;
                    break;
                case 0x01:
                    State = SocketState.On;
                    break;
                default:
                    throw new MessageException($"Error parsing message: 0x{state:X2} is not a valid state.");
            }
        }
    }
}
