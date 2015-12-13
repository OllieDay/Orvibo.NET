using System;
using System.Linq;
using System.Net.NetworkInformation;
using Orvibo.Extensions;

namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Provides the base implementation of an inbound Orvibo message.
    /// </summary>
    public abstract class InboundMessage : IInboundMessage
    {
        /// <summary>
        ///     Length of MAC address bytes.
        /// </summary>
        private const int MacAddressLength = 6;

        /// <summary>
        ///     Minimum message length.
        /// </summary>
        private const int MinMessageLength = 6;

        /// <summary>
        ///     First 2 bytes of an Orvibo device MAC address.
        /// </summary>
        private static readonly byte[] DeviceIdentifier = { 0xAC, 0xCF };

        /// <summary>
        ///     Message data.
        /// </summary>
        private readonly byte[] _data;

        /// <summary>
        ///     Initializes a new instance of the InboundMessage class.
        /// </summary>
        /// <param name="data">Message data.</param>
        protected InboundMessage(byte[] data)
        {
            _data = data;

            Validate();
            ParseMacAddress();
            ParseState();
        }

        /// <summary>
        ///     Gets the socket MAC address.
        /// </summary>
        public PhysicalAddress MacAddress { get; private set; }

        /// <summary>
        ///     Gets the socket state.
        /// </summary>
        public SocketState State { get; private set; }

        /// <summary>
        ///     Gets the message length.
        /// </summary>
        protected abstract ushort Length { get; }

        /// <summary>
        ///     Parses the message data for the socket MAC address.
        /// </summary>
        private void ParseMacAddress()
        {
            try
            {
                var index = _data.IndexOfSequence(DeviceIdentifier).First();
                var bytes = _data.Skip(index).Take(MacAddressLength).ToArray();

                if (bytes.Length != MacAddressLength)
                {
                    throw new InvalidOperationException();
                }

                MacAddress = new PhysicalAddress(bytes);
            }
            catch (InvalidOperationException)
            {
                throw new MessageException("Error parsing message: source MAC address not present.");
            }
        }

        /// <summary>
        ///     Parses the message data for the socket state.
        /// </summary>
        private void ParseState()
        {
            var state = _data.Last();

            switch (state)
            {
                case 0x00:
                    State = SocketState.Off;
                    break;
                case 0x01:
                    State = SocketState.On;
                    break;
                default:
                    throw new MessageException($"Error parsing message: 0x{state:X2} is not a valid state.");
            }
        }

        /// <summary>
        ///     Validates the message data, ensuring its length is at least the minimum message length.
        /// </summary>
        private void Validate()
        {
            if (_data.Length < MinMessageLength)
            {
                throw new MessageException($"Error parsing message: expected length {MinMessageLength}+, actual length {_data.Length}.");
            }
        }
    }
}
