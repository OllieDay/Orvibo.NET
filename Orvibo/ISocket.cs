using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Orvibo
{
    public interface ISocket
    {
        event EventHandler<EventArgs> StateChanged;

        event EventHandler<EventArgs> Subscribed;

        event EventHandler<EventArgs> Unsubscribed;

        IPAddress IPAddress { get; }

        bool IsSubscribed { get; }

        PhysicalAddress MacAddress { get; }

        SocketState State { get; }

        void Off();

        void On();

        void Subscribe();

        void Unsubscribe();
    }
}
