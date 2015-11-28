namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Provides functionality for sending outgoing Orvibo messages to a single device.
    /// </summary>
    internal interface IUnicastMessageSender
    {
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
