using System.Net.NetworkInformation;

namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Broadcast to the network to discover a socket with a known MAC address. This is needed when the IP address of the
    ///     socket is not known or has changed.
    /// </summary>
    internal sealed class OutboundRediscoveryMessage : OutboundAddressableMessage
    {
        /// <summary>
        ///     Initializes a new instance of the OutboundRediscoveryMessage class.
        /// </summary>
        /// <param name="macAddress">Socket MAC address.</param>
        public OutboundRediscoveryMessage(PhysicalAddress macAddress) : base(macAddress) {}

        /// <summary>
        ///     Gets the command for the message type.
        /// </summary>
        protected override byte[] Command { get; } = { 0x71, 0x67 };
    }
}
