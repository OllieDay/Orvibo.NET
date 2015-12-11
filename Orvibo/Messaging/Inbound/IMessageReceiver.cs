using System;
using System.Collections.Generic;
using System.Net;

namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Provides functionality for receiving inbound messages.
    /// </summary>
    internal interface IMessageReceiver : IDisposable
    {
        /// <summary>
        ///     Raised when a message is received.
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        ///     Gets the list IP addresses that should be ignore when receiving messages.
        /// </summary>
        List<IPAddress> Blacklist { get; set; }

        /// <summary>
        ///     Gets the running status.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        ///     Starts the message receiver.
        /// </summary>
        void Start();

        /// <summary>
        ///     Stops the message receiver.
        /// </summary>
        void Stop();
    }
}
