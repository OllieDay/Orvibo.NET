using System.Linq;
using System.Net.NetworkInformation;

namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Sent to a socket to subscribe to events.
    /// </summary>
    internal sealed class OutboundSubscribeMessage : OutboundAddressableMessage
    {
        /// <summary>
        ///     Initializes a new instance of the OutboundSubscribeMessage class.
        /// </summary>
        /// <param name="macAddress">Socket MAC address.</param>
        public OutboundSubscribeMessage(PhysicalAddress macAddress) : base(macAddress) {}

        /// <summary>
        ///     Gets the command for the message type.
        /// </summary>
        protected override byte[] Command { get; } = { 0x63, 0x6C };

        /// <summary>
        ///     Gets the data for the message type.
        /// </summary>
        /// <returns>Data for the messae type.</returns>
        protected override byte[] GetData()
        {
            return Combine(base.GetData(), Padding, GetLittleEndianMacAddressBytes());
        }

        /// <summary>
        ///     Gets the MAC address bytes in little endian format.
        /// </summary>
        /// <returns>MAC address bytes in little endian format.</returns>
        private byte[] GetLittleEndianMacAddressBytes()
        {
            return GetMacAddressBytes().Reverse().ToArray();
        }
    }
}
