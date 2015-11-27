using System.Net;
using System.Net.NetworkInformation;

namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Provides functionality for broadcasting outgoing Orvibo messages.
    /// </summary>
    internal sealed class BroadcastMessageSender : IBroadcastMessageSender
    {
        /// <summary>
        ///     Used for sending messages.
        /// </summary>
        private readonly IMessageSender _messageSender;

        /// <summary>
        ///     Initializes a new instance of the BroadcastMessageSender class.
        /// </summary>
        /// <param name="messageSender">Message sender used for sending messages.</param>
        public BroadcastMessageSender(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        /// <summary>
        ///     Broadcasts a discovery message to the network.
        /// </summary>
        public void SendDiscoveryMessage()
        {
            _messageSender.Send(new OutboundDiscoveryMessage(), IPAddress.Broadcast);
        }

        /// <summary>
        ///     Broadcasts a rediscovery message with the specified socket MAC address to the network.
        /// </summary>
        /// <param name="macAddress">Socket MAC address</param>
        public void SendRediscoveryMessage(PhysicalAddress macAddress)
        {
            _messageSender.Send(new OutboundRediscoveryMessage(macAddress), IPAddress.Broadcast);
        }
    }
}
