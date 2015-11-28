namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Provides functionality for sending outgoing Orvibo messages to a single device.
    /// </summary>
    internal sealed class UnicastMessageSender : IUnicastMessageSender
    {
        /// <summary>
        ///     Used for sending messages.
        /// </summary>
        private readonly IMessageSender _messageSender;

        /// <summary>
        ///     Initializes a new instance of the UnicastMessageSender class.
        /// </summary>
        /// <param name="messageSender">Message sender used for sending messages.</param>
        public UnicastMessageSender(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        /// <summary>
        ///     Sends an off message to the specified destination.
        /// </summary>
        /// <param name="destination">Device to send the message to.</param>
        public void SendOffMessage(INetworkDevice destination)
        {
            _messageSender.Send(new OutboundStateChangeMessage(destination.MacAddress, SocketState.Off), destination.IPAddress);
        }

        /// <summary>
        ///     Sends an on message to the specified destination.
        /// </summary>
        /// <param name="destination">Device to send the message to.</param>
        public void SendOnMessage(INetworkDevice destination)
        {
            _messageSender.Send(new OutboundStateChangeMessage(destination.MacAddress, SocketState.On), destination.IPAddress);
        }

        /// <summary>
        ///     Sends a subscribe message to the specified destination.
        /// </summary>
        /// <param name="destination">Device to send the message to.</param>
        public void SendSubscribeMessage(INetworkDevice destination)
        {
            _messageSender.Send(new OutboundSubscribeMessage(destination.MacAddress), destination.IPAddress);
        }
    }
}
