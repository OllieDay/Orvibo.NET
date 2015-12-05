using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Orvibo
{
    /// <summary>
    ///     Represents an Orvibo socket.
    /// </summary>
    public interface ISocket : IDisposable
    {
        /// <summary>
        ///     Raised when the state changes.
        /// </summary>
        event EventHandler<SocketStateChangedEventArgs> StateChanged;

        /// <summary>
        ///     Raised when the socket is subscribed.
        /// </summary>
        event EventHandler<EventArgs> Subscribed;

        /// <summary>
        ///     Raised when the socket is unsubscribed.
        /// </summary>
        event EventHandler<EventArgs> Unsubscribed;

        /// <summary>
        ///     IP address.
        /// </summary>
        IPAddress IPAddress { get; }

        /// <summary>
        ///     Gets the subscribed state.
        /// </summary>
        bool IsSubscribed { get; }

        /// <summary>
        ///     MAC address.
        /// </summary>
        PhysicalAddress MacAddress { get; }

        /// <summary>
        ///     Socket state.
        /// </summary>
        SocketState State { get; }

        /// <summary>
        ///     Switches the socket off.
        /// </summary>
        void Off();

        /// <summary>
        ///     Switches the socket on.
        /// </summary>
        void On();

        /// <summary>
        ///     Subscribes the socket. Must be called prior to switching off or on.
        /// </summary>
        void Subscribe();

        /// <summary>
        ///     Unsubscribes the socket.
        /// </summary>
        void Unsubscribe();
    }
}
