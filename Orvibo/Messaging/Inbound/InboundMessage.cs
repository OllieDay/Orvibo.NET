using System;
using System.Linq;
using System.Net.NetworkInformation;
using Orvibo.Extensions;

namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Provides the base implementation of an inbound Orvibo message.
    /// </summary>
    internal abstract class InboundMessage : IInboundMessage
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
        ///     Initializes a new instance of the InboundMessage class.
        /// </summary>
        /// <param name="data">Message data.</param>
        protected InboundMessage(byte[] data)
        {
            Data = data;

            Validate();
            ParseMacAddress();
        }

        /// <summary>
        ///     Gets the socket MAC address.
        /// </summary>
        public PhysicalAddress MacAddress { get; private set; }

        /// <summary>
        ///     Gets the message data.
        /// </summary>
        protected byte[] Data { get; }

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
                var index = Data.IndexOfSequence(DeviceIdentifier).First();
                var bytes = Data.Skip(index).Take(MacAddressLength).ToArray();

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
        ///     Validates the message data, ensuring its length is at least the minimum message length.
        /// </summary>
        private void Validate()
        {
            if (Data.Length < MinMessageLength)
            {
                throw new MessageException($"Error parsing message: expected length {MinMessageLength}+, actual length {Data.Length}.");
            }
        }
    }
}
