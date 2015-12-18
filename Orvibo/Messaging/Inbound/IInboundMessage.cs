using System.Net.NetworkInformation;

namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Represents an inbound Orvibo message.
    /// </summary>
    internal interface IInboundMessage
    {
        /// <summary>
        ///     Gets the socket MAC address.
        /// </summary>
        PhysicalAddress MacAddress { get; }
    }
}
