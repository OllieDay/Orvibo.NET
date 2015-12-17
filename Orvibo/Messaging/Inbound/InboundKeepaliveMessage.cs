namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Received from a socket after sending an outbound keepalive message.
    /// </summary>
    internal sealed class InboundKeepaliveMessage : InboundMessage
    {
        /// <summary>
        ///     Initializes a new instance of the InboundKeepaliveMessage class.
        /// </summary>
        /// <param name="data">Message data.</param>
        public InboundKeepaliveMessage(byte[] data) : base(data) {}

        /// <summary>
        ///     Gets the message length.
        /// </summary>
        protected override ushort Length { get; } = 23;
    }
}
