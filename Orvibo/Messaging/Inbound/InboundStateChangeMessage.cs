namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Received from a socket when its state changes.
    /// </summary>
    internal sealed class InboundStateChangeMessage : InboundStateMessage
    {
        /// <summary>
        ///     Initializes a new instance of the InboundStateChangeMessage class.
        /// </summary>
        /// <param name="data">Message data.</param>
        public InboundStateChangeMessage(byte[] data) : base(data) {}

        /// <summary>
        ///     Gets the message length.
        /// </summary>
        protected override ushort Length { get; } = 23;
    }
}
