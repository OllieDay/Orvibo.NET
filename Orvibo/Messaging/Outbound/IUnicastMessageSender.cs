using System;

namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Provides functionality for sending outbound Orvibo messages to a single device.
    /// </summary>
    internal interface IUnicastMessageSender : IDisposable
    {
        /// <summary>
        ///     Sends a keepalive message to the specified destination.
        /// </summary>
        /// <param name="destination">Device to send the message to.</param>
        void SendKeepaliveMessage(INetworkDevice destination);

        /// <summary>
        ///     Sends an off message to the specified destination.
        /// </summary>
        /// <param name="destination">Device to send the message to.</param>
        void SendOffMessage(INetworkDevice destination);

        /// <summary>
        ///     Sends an on message to the specified destination.
        /// </summary>
        /// <param name="destination">Device to send the message to.</param>
        void SendOnMessage(INetworkDevice destination);

        /// <summary>
        ///     Sends a subscribe message to the specified destination.
        /// </summary>
        /// <param name="destination">Device to send the message to.</param>
        void SendSubscribeMessage(INetworkDevice destination);
    }
}
