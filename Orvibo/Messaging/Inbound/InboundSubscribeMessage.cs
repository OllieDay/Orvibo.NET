namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Received from a socket after sending an outbound subscribe message.
    /// </summary>
    internal sealed class InboundSubscribeMessage : InboundStateMessage
    {
        /// <summary>
        ///     Initializes a new instance of the InboundSubscribeMessage class.
        /// </summary>
        /// <param name="data">Message data.</param>
        public InboundSubscribeMessage(byte[] data) : base(data) {}

        /// <summary>
        ///     Gets the message length.
        /// </summary>
        protected override ushort Length { get; } = 24;
    }
}
