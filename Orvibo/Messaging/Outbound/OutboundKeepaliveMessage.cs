using System.Net.NetworkInformation;

namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Sent to a socket to maintain subscription. If 5 minutes elapses with no message sent to a socket then the
    ///     subscription is cancelled.
    /// </summary>
    internal sealed class OutboundKeepaliveMessage : OutboundAddressableMessage
    {
        /// <summary>
        ///     Message data.
        /// </summary>
        private static readonly byte[] Data = { 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        ///     Initializes a new instance of the OutboundKeepaliveMessage class.
        /// </summary>
        /// <param name="macAddress">Socket MAC address.</param>
        public OutboundKeepaliveMessage(PhysicalAddress macAddress) : base(macAddress) {}

        /// <summary>
        ///     Gets the command for the message type.
        /// </summary>
        protected override byte[] Command { get; } = { 0x68, 0x62 };

        /// <summary>
        ///     Gets the data for the message type.
        /// </summary>
        /// <returns>Data for the messae type.</returns>
        protected override byte[] GetData()
        {
            return Combine(base.GetData(), Data);
        }
    }
}
