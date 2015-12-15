namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Received from a socket after broadcasting an outbound discovery message.
    /// </summary>
    internal sealed class InboundDiscoveryMessage : InboundMessage
    {
        /// <summary>
        ///     Initializes a new instance of the InboundDiscoveryMessage class.
        /// </summary>
        /// <param name="data">Message data.</param>
        public InboundDiscoveryMessage(byte[] data) : base(data) {}

        /// <summary>
        ///     Gets the message length.
        /// </summary>
        protected override ushort Length { get; } = 42;
    }
}
