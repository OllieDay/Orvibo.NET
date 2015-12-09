using System.Net;
using System.Net.NetworkInformation;

namespace Orvibo
{
    /// <summary>
    ///     Represents a network device with a MAC address and IP address.
    /// </summary>
    internal interface INetworkDevice
    {
        /// <summary>
        ///     Gets the IP address.
        /// </summary>
        IPAddress IPAddress { get; }

        /// <summary>
        ///     Gets the MAC address.
        /// </summary>
        PhysicalAddress MacAddress { get; }
    }
}
