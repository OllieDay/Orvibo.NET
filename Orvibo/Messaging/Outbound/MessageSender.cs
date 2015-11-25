using System;
using System.Net;
using System.Net.Sockets;

namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Provides functionality for sending outgoing Orvibo messages.
    /// </summary>
    internal sealed class MessageSender : IMessageSender, IDisposable
    {
        /// <summary>
        ///     Orvibo socket port.
        /// </summary>
        private const int Port = 10000;

        /// <summary>
        ///     Client for sending UDP packets.
        /// </summary>
        private readonly UdpClient _client;

        /// <summary>
        ///     Initializes a new instance of the MessageSender class.
        /// </summary>
        public MessageSender()
        {
            try
            {
                _client = new UdpClient();
            }
            catch (SocketException e)
            {
                throw new OrviboException("Error creating UDP client.", e);
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable) _client).Dispose();
        }

        /// <summary>
        ///     Sends the specified message to the specified destination.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <param name="destination">Destination to send message to.</param>
        public void Send(IOutboundMessage message, IPAddress destination)
        {
            var endPoint = new IPEndPoint(destination, Port);
            var payload = message.GetPayload();

            try
            {
                var sent = _client.Send(payload, payload.Length, endPoint);

                if (sent != payload.Length)
                {
                    throw new MessageException($"Error sending message: expected bytes sent {payload.Length}, actual bytes sent {sent}.");
                }
            }
            catch (SocketException e)
            {
                throw new MessageException("Error sending message.", e);
            }
        }
    }
}
