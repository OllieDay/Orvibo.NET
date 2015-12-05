using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Orvibo.Messaging.Inbound;
using Orvibo.Messaging.Outbound;

namespace Orvibo
{
    internal sealed class Socket : ISocket, INetworkDevice
    {
        private readonly IUnicastMessageSender _messageSender;

        private bool _isSubscribed;

        private SocketState _state = SocketState.Unknown;

        public Socket(PhysicalAddress macAddress, IPAddress ipAddress, IUnicastMessageSender messageSender)
        {
            MacAddress = macAddress;
            IPAddress = ipAddress;
            _messageSender = messageSender;
        }

        public event EventHandler<SocketStateChangedEventArgs> StateChanged;

        public event EventHandler<EventArgs> Subscribed;

        public event EventHandler<EventArgs> Unsubscribed;

        public IPAddress IPAddress { get; }

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
                        OnSubscribed(EventArgs.Empty);
                    }
                    else
                    {
                        OnUnsubscribed(EventArgs.Empty);
                    }
                }
            }
        }

        public PhysicalAddress MacAddress { get; }

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

        public void Off()
        {
            ChangeState(SocketState.Off);
        }

        public void On()
        {
            ChangeState(SocketState.On);
        }

        public void Process(InboundSubscribeMessage message)
        {
            IsSubscribed = true;
            Process((IInboundMessage) message);
        }

        public void Process(InboundStateChangeMessage message)
        {
            Process((IInboundMessage) message);
        }

        public void Subscribe()
        {
            if (IsSubscribed)
            {
                throw new InvalidOperationException("Already subscribed.");
            }

            _messageSender.SendSubscribeMessage(this);
        }

        public void Unsubscribe()
        {
            if (!IsSubscribed)
            {
                throw new InvalidOperationException("Not subscribed");
            }

            IsSubscribed = false;
        }

        private void ChangeState(SocketState state)
        {
            if (!IsSubscribed)
            {
                throw new InvalidOperationException("Not subscribed.");
            }

            if (State == state)
            {
                throw new InvalidOperationException($"State already {state}.");
            }

            if (state == SocketState.Off)
            {
                _messageSender.SendOffMessage(this);
            }
            else if (state == SocketState.On)
            {
                _messageSender.SendOnMessage(this);
            }
        }

        private void OnStateChanged(SocketStateChangedEventArgs e)
        {
            Volatile.Read(ref StateChanged)?.Invoke(this, e);
        }

        private void OnSubscribed(EventArgs e)
        {
            Volatile.Read(ref Subscribed)?.Invoke(this, e);
        }

        private void OnUnsubscribed(EventArgs e)
        {
            Volatile.Read(ref Unsubscribed)?.Invoke(this, e);
        }

        private void Process(IInboundMessage message)
        {
            State = message.State;
        }
    }
}
