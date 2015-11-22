using System;
using System.Net.NetworkInformation;

namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Sent to a socket to change its state to off or on.
    /// </summary>
    internal sealed class OutboundStateChangeMessage : OutboundAddressableMessage
    {
        /// <summary>
        ///     Padding preceding state.
        /// </summary>
        private static readonly byte[] StatePadding = { 0x00, 0x00, 0x00, 0x00 };

        /// <summary>
        ///     State to change to.
        /// </summary>
        private readonly SocketState _state;

        /// <summary>
        ///     Initializes a new instance of the OutboundStateChangeMessage class.
        /// </summary>
        /// <param name="macAddress">Socket MAC address.</param>
        /// <param name="state">State to change to.</param>
        public OutboundStateChangeMessage(PhysicalAddress macAddress, SocketState state) : base(macAddress)
        {
            if (state != SocketState.Off && state != SocketState.On)
            {
                throw new ArgumentOutOfRangeException(nameof(state), state, $"Must be {SocketState.Off} or {SocketState.On}.");
            }

            _state = state;
        }

        /// <summary>
        ///     Command for the message type.
        /// </summary>
        protected override byte[] Command { get; } = { 0x64, 0x63 };

        /// <summary>
        ///     Gets the data for the message type.
        /// </summary>
        /// <returns>Data for the messae type.</returns>
        protected override byte[] GetData()
        {
            return Combine(base.GetData(), StatePadding, GetStateBytes());
        }

        /// <summary>
        ///     Gets the state bytes.
        /// </summary>
        /// <returns>State bytes.</returns>
        private byte[] GetStateBytes()
        {
            return new[] { (byte) (_state == SocketState.Off ? 0 : 1) };
        }
    }
}
