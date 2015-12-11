using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Provides functionality for receiving inbound messages.
    /// </summary>
    internal sealed class MessageReceiver : IMessageReceiver
    {
        /// <summary>
        ///     Orvibo socket port.
        /// </summary>
        private const int Port = 10000;

        /// <summary>
        ///     Client for receiving UDP packets.
        /// </summary>
        private readonly UdpClient _client;

        /// <summary>
        ///     Message parser.
        /// </summary>
        private readonly IMessageParser _messageParser;

        /// <summary>
        ///     Initializes a new instance of the MessageReceiver class with an empty blacklist.
        /// </summary>
        /// <param name="messageParser">Message parser.</param>
        public MessageReceiver(IMessageParser messageParser) : this(messageParser, new List<IPAddress>()) {}

        /// <summary>
        ///     Initializes a new instance of the MessageReceiver class.
        /// </summary>
        /// <param name="messageParser">Message parser.</param>
        /// <param name="blacklist">IP addresses that should be ignore when receiving messages.</param>
        public MessageReceiver(IMessageParser messageParser, List<IPAddress> blacklist)
        {
            _messageParser = messageParser;
            Blacklist = blacklist;

            _client = new UdpClient(Port);
        }

        /// <summary>
        ///     Raised when a message is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        ///     Gets the list IP addresses that should be ignore when receiving messages.
        /// </summary>
        public List<IPAddress> Blacklist { get; set; }

        /// <summary>
        ///     Gets the running status.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Stop();
            ((IDisposable) _client).Dispose();
        }

        /// <summary>
        ///     Starts the message receiver.
        /// </summary>
        public void Start()
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("Already running.");
            }

            IsRunning = true;
            Task.Run(() => Receive());
        }

        /// <summary>
        ///     Stops the message receiver.
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
        }

        /// <summary>
        ///     Determines whether or not the specified IP address is blacklisted.
        /// </summary>
        /// <param name="ipAddress">IP address to check.</param>
        /// <returns>True if the specified IP address is blacklisted, otherwise false.</returns>
        private bool IsBlacklisted(IPAddress ipAddress)
        {
            return Blacklist.Contains(ipAddress);
        }

        /// <summary>
        ///     Invokes the MessageReceived event.
        /// </summary>
        /// <param name="e">Event args.</param>
        private void OnMessageReceived(MessageReceivedEventArgs e)
        {
            Volatile.Read(ref MessageReceived)?.Invoke(this, e);
        }

        /// <summary>
        ///     Receives inbound message while the message receiver is running.
        /// </summary>
        private void Receive()
        {
            var endPoint = new IPEndPoint(IPAddress.Any, Port);

            while (IsRunning)
            {
                try
                {
                    var received = _client.Receive(ref endPoint);
                    var ip = endPoint.Address;

                    if (!IsBlacklisted(ip))
                    {
                        try
                        {
                            var message = _messageParser.Parse(received);

                            OnMessageReceived(new MessageReceivedEventArgs(message));
                        }
                        catch (MessageException)
                        {
                            // Failed to parse message.
                        }
                    }
                }
                catch
                {
                    Stop();

                    throw;
                }
            }
        }
    }
}
