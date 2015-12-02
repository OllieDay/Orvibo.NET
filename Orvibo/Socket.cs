using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Orvibo.Messaging.Outbound;

namespace Orvibo
{
    internal sealed class Socket : ISocket, INetworkDevice
    {
        private readonly IUnicastMessageSender _messageSender;

        public Socket(PhysicalAddress macAddress, IPAddress ipAddress, IUnicastMessageSender messageSender)
        {
            MacAddress = macAddress;
            IPAddress = ipAddress;
            _messageSender = messageSender;
        }

        public event EventHandler<EventArgs> StateChanged;

        public event EventHandler<EventArgs> Subscribed;

        public event EventHandler<EventArgs> Unsubscribed;

        public IPAddress IPAddress { get; }

        public bool IsSubscribed { get; private set; }

        public PhysicalAddress MacAddress { get; }

        public SocketState State { get; private set; } = SocketState.Unknown;

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _messageSender.Dispose();
        }

        public void Off()
        {
            _messageSender.SendOffMessage(this);
        }

        public void On()
        {
            _messageSender.SendOnMessage(this);
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
            OnUnsubscribed(EventArgs.Empty);
        }

        private void OnStateChanged(EventArgs e)
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
    }
}
