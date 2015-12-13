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
    public static class SocketFactory
    {
        /// <summary>
        ///     Message receiver.
        /// </summary>
        private static readonly IMessageReceiver MessageReceiver = new MessageReceiver(new MessageParser());

        /// <summary>
        ///     Message sender.
        /// </summary>
        private static readonly IMessageSender MessageSender = new MessageSender();

        /// <summary>
        ///     List of sockets registered to receive messages.
        /// </summary>
        private static readonly List<Socket> Sockets = new List<Socket>();

        /// <summary>
        ///     Creates a new socket with the specified MAC address and IP address.
        /// </summary>
        /// <param name="macAddress">MAC address.</param>
        /// <param name="ipAddress">IP address.</param>
        /// <returns>Created socket.</returns>
        public static ISocket Create(PhysicalAddress macAddress, IPAddress ipAddress)
        {
            var socket = new Socket(macAddress, ipAddress, new UnicastMessageSender(MessageSender));

            socket.Subscribing += SocketSubscribing;
            socket.Unsubscribing += SocketUnsubscribing;

            return socket;
        }

        /// <summary>
        ///     IMessageReceiver.MessageReceived event callback.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event args.</param>
        private static void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Sockets.FirstOrDefault(s => s.MacAddress.Equals(e.Message.MacAddress))?.Process(e.Message);
        }

        /// <summary>
        ///     ISocket.Subscribing callback.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event args.</param>
        private static void SocketSubscribing(object sender, EventArgs e)
        {
            if (!Sockets.Any())
            {
                MessageReceiver.Start();
            }

            Sockets.Add((Socket) sender);
        }

        /// <summary>
        ///     ISocket.Unsubscribing callback.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event args.</param>
        private static void SocketUnsubscribing(object sender, EventArgs e)
        {
            Sockets.Remove((Socket) sender);

            if (!Sockets.Any())
            {
                MessageReceiver.Stop();
            }
        }
    }
}
