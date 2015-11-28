using System.Net.NetworkInformation;

namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Provides functionality for broadcasting outbound Orvibo messages.
    /// </summary>
    internal interface IBroadcastMessageSender
    {
        /// <summary>
        ///     Broadcasts a discovery message to the network.
        /// </summary>
        void SendDiscoveryMessage();

        /// <summary>
        ///     Broadcasts a rediscovery message with the specified socket MAC address to the network.
        /// </summary>
        /// <param name="macAddress">Socket MAC address</param>
        void SendRediscoveryMessage(PhysicalAddress macAddress);
    }
}
