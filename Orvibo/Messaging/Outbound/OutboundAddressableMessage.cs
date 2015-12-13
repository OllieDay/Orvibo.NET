using System.Net.NetworkInformation;

namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Provides a base implementation of a message sent to a socket with a known MAC address.
    /// </summary>
    internal abstract class OutboundAddressableMessage : OutboundMessage
    {
        /// <summary>
        ///     Initializes a new instance of the OutboundAddressableMessage class.
        /// </summary>
        /// <param name="macAddress">Socket MAC address.</param>
        protected OutboundAddressableMessage(PhysicalAddress macAddress)
        {
            MacAddress = macAddress;
        }

        /// <summary>
        ///     Gets the padding appended to MAC address.
        /// </summary>
        protected static byte[] Padding { get; } = { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

        /// <summary>
        ///     Gets the command for the message type.
        /// </summary>
        protected override byte[] Command { get; } = new byte[0];

        /// <summary>
        ///     Gets the socket MAC address.
        /// </summary>
        private PhysicalAddress MacAddress { get; }

        /// <summary>
        ///     Gets the data for the message type.
        /// </summary>
        /// <returns>Data for the messae type.</returns>
        protected override byte[] GetData()
        {
            return Combine(base.GetData(), GetMacAddressBytes(), Padding);
        }

        /// <summary>
        ///     Gets the MAC address bytes.
        /// </summary>
        /// <returns>MAC address bytes.</returns>
        protected byte[] GetMacAddressBytes()
        {
            return MacAddress.GetAddressBytes();
        }
    }
}
