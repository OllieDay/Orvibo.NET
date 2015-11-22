namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Broadcast to the network to discover any Orvibo sockets.
    /// </summary>
    internal sealed class OutboundDiscoveryMessage : OutboundMessage
    {
        /// <summary>
        ///     Command for the message type.
        /// </summary>
        protected override byte[] Command { get; } = { 0x71, 0x61 };
    }
}
