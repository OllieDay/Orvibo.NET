namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Received from a socket after broadcasting an outbound rediscovery message.
    /// </summary>
    internal sealed class InboundRediscoveryMessage : InboundStateMessage
    {
        /// <summary>
        ///     Initializes a new instance of the InboundRediscoveryMessage class.
        /// </summary>
        /// <param name="data">Message data.</param>
        public InboundRediscoveryMessage(byte[] data) : base(data) {}

        /// <summary>
        ///     Gets the message length.
        /// </summary>
        protected override ushort Length { get; } = 42;
    }
}
