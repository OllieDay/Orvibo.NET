namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Event args for the message received event.
    /// </summary>
    internal sealed class MessageReceivedEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the MessageReceivedEventArgs class.
        /// </summary>
        /// <param name="message">Received message.</param>
        public MessageReceivedEventArgs(IInboundMessage message)
        {
            Message = message;
        }

        /// <summary>
        ///     Gets the received message.
        /// </summary>
        public IInboundMessage Message { get; }
    }
}
