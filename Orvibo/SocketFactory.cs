using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Orvibo.Messaging.Inbound;
using Orvibo.Messaging.Outbound;

namespace Orvibo
{
    /// <summary>
    ///     Provides functionality for creating sockets.
    /// </summary>
    public class SocketFactory : IDisposable
    {
        /// <summary>
        ///     Message receiver.
        /// </summary>
        private readonly IMessageReceiver _messageReceiver;

        /// <summary>
        ///     Message sender.
        /// </summary>
        private readonly IMessageSender _messageSender;

        /// <summary>
        ///     List of sockets registered to receive messages.
        /// </summary>
        private readonly List<Socket> _sockets = new List<Socket>();

        /// <summary>
        ///     Creates a new instance of the SocketFactory class.
        /// </summary>
        public SocketFactory() : this(new MessageReceiver(new MessageParser()), new MessageSender()) {}

        /// <summary>
        ///     Creates a new instance of the SocketFactory class.
        /// </summary>
        /// <param name="messageReceiver">Message receiver.</param>
        /// <param name="messageSender">Message sender.</param>
        internal SocketFactory(IMessageReceiver messageReceiver, IMessageSender messageSender)
        {
            _messageReceiver = messageReceiver;
            _messageSender = messageSender;

            _messageReceiver.MessageReceived += MessageReceived;
        }

        /// <summary>
        ///     Creates a new socket with the specified MAC address and IP address.
        /// </summary>
        /// <param name="macAddress">MAC address.</param>
        /// <param name="ipAddress">IP address.</param>
        /// <returns></returns>
        public ISocket Create(PhysicalAddress macAddress, IPAddress ipAddress)
        {
            var socket = new Socket(macAddress, ipAddress, new UnicastMessageSender(_messageSender));

            socket.Subscribing += SocketSubscribing;
            socket.Unsubscribing += SocketUnsubscribing;

            return socket;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _messageReceiver.Dispose();
            _messageSender.Dispose();
        }

        /// <summary>
        ///     IMessageReceiver.MessageReceived event callback.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event args.</param>
        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            _sockets.FirstOrDefault(s => s.MacAddress.Equals(e.Message.MacAddress))?.Process(e.Message);
        }

        /// <summary>
        ///     ISocket.Subscribing callback.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event args.</param>
        private void SocketSubscribing(object sender, EventArgs e)
        {
            if (!_sockets.Any())
            {
                _messageReceiver.Start();
            }

            _sockets.Add((Socket) sender);
        }

        /// <summary>
        ///     ISocket.Unsubscribing callback.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event args.</param>
        private void SocketUnsubscribing(object sender, EventArgs e)
        {
            _sockets.Remove((Socket) sender);

            if (!_sockets.Any())
            {
                _messageReceiver.Stop();
            }
        }
    }
}
