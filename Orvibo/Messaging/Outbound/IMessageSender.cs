using System.Net;

namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Provides functionality for sending outgoing Orvibo messages.
    /// </summary>
    internal interface IMessageSender
    {
        /// <summary>
        ///     Sends the specified message to the specified destination.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <param name="destination">Destination to send message to.</param>
        void Send(IOutboundMessage message, IPAddress destination);
    }
}
