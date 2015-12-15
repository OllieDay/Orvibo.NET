using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Orvibo.Messaging.Inbound;
using Orvibo.Messaging.Outbound;

namespace Orvibo
{
    /// <summary>
    ///     Represents an Orvibo socket.
    /// </summary>
    internal sealed class Socket : ISocket, INetworkDevice
    {
        /// <summary>
        ///     Used for sending messages.
        /// </summary>
        private readonly IUnicastMessageSender _messageSender;

        /// <summary>
        ///     IsSubscribed backing field.
        /// </summary>
        private bool _isSubscribed;

        /// <summary>
        ///     State backing field.
        /// </summary>
        private SocketState _state = SocketState.Unknown;

        /// <summary>
        ///     Initializes a new instance of the Socket class.
        /// </summary>
        /// <param name="macAddress">MAC address.</param>
        /// <param name="ipAddress">IP address.</param>
        /// <param name="messageSender">Used for sending messages.</param>
        public Socket(PhysicalAddress macAddress, IPAddress ipAddress, IUnicastMessageSender messageSender)
        {
            MacAddress = macAddress;
            IPAddress = ipAddress;
            _messageSender = messageSender;
        }

        /// <summary>
        ///     Raised when the state changes.
        /// </summary>
        public event EventHandler<SocketStateChangedEventArgs> StateChanged;

        /// <summary>
        ///     Raised when the socket is subscribed.
        /// </summary>
        public event EventHandler Subscribed;

        /// <summary>
        ///     Raised when the socket is unsubscribed.
        /// </summary>
        public event EventHandler Unsubscribed;

        /// <summary>
        ///     Raised when subscribing.
        /// </summary>
        internal event EventHandler Subscribing;

        /// <summary>
        ///     Raised when unsubscribing.
        /// </summary>
        internal event EventHandler Unsubscribing;

        /// <summary>
        ///     Gets the IP address.
        /// </summary>
        public IPAddress IPAddress { get; }

        /// <summary>
        ///     Gets the subscribed state.
        /// </summary>
        public bool IsSubscribed
        {
            get
            {
                return _isSubscribed;
            }

            private set
            {
                if (IsSubscribed != value)
                {
                    _isSubscribed = value;

                    if (IsSubscribed)
                    {
                        OnSubscribed();
                    }
                    else
                    {
                        OnUnsubscribed();
                    }
                }
            }
        }

        /// <summary>
        ///     Gets the MAC address.
        /// </summary>
        public PhysicalAddress MacAddress { get; }

        /// <summary>
        ///     Gets the socket state.
        /// </summary>
        public SocketState State
        {
            get
            {
                return _state;
            }

            private set
            {
                if (State != value)
                {
                    var fromState = State;
                    _state = value;

                    OnStateChanged(new SocketStateChangedEventArgs(fromState, value));
                }
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _messageSender.Dispose();
        }

        /// <summary>
        ///     Switches the socket off.
        /// </summary>
        public void Off()
        {
            ChangeState(SocketState.Off);
        }

        /// <summary>
        ///     Switches the socket on.
        /// </summary>
        public void On()
        {
            ChangeState(SocketState.On);
        }

        /// <summary>
        ///     Processes the inbound message.
        /// </summary>
        /// <param name="message">Message to process.</param>
        public void Process(IInboundMessage message)
        {
            var subscribeMessage = message as InboundSubscribeMessage;

            if (subscribeMessage != null)
            {
                IsSubscribed = true;
            }

            State = message.State;
        }

        /// <summary>
        ///     Subscribes the socket. Must be called prior to switching off or on.
        /// </summary>
        public void Subscribe()
        {
            if (IsSubscribed)
            {
                throw new InvalidOperationException("Already subscribed.");
            }

            OnSubscribing();
            _messageSender.SendSubscribeMessage(this);
        }

        /// <summary>
        ///     Unsubscribes the socket.
        /// </summary>
        public void Unsubscribe()
        {
            if (!IsSubscribed)
            {
                throw new InvalidOperationException("Not subscribed.");
            }

            OnUnsubscribing();
            IsSubscribed = false;
        }

        /// <summary>
        ///     Changes the socket state.
        /// </summary>
        /// <param name="state">State to change to.</param>
        private void ChangeState(SocketState state)
        {
            if (!IsSubscribed)
            {
                throw new InvalidOperationException("Not subscribed.");
            }

            if (State == state)
            {
                throw new InvalidOperationException($"Already {state}.");
            }

            if (state == SocketState.Off)
            {
                _messageSender.SendOffMessage(this);
            }
            else
            {
                _messageSender.SendOnMessage(this);
            }
        }

        /// <summary>
        ///     Raises the StateChanged event.
        /// </summary>
        /// <param name="e">Event args.</param>
        private void OnStateChanged(SocketStateChangedEventArgs e)
        {
            Volatile.Read(ref StateChanged)?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the Subscribed event.
        /// </summary>
        private void OnSubscribed()
        {
            Volatile.Read(ref Subscribed)?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Invokes the Subscribing event.
        /// </summary>
        private void OnSubscribing()
        {
            Volatile.Read(ref Subscribing)?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Raises the Unsubscribed event.
        /// </summary>
        private void OnUnsubscribed()
        {
            Volatile.Read(ref Unsubscribed)?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Invokes the Unsubscribing event.
        /// </summary>
        private void OnUnsubscribing()
        {
            Volatile.Read(ref Unsubscribing)?.Invoke(this, EventArgs.Empty);
        }
    }
}
