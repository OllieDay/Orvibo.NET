using System;
using System.Linq;
using Orvibo.Extensions;

namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Parses inbound Orvibo messages.
    /// </summary>
    internal sealed class MessageParser : IMessageParser
    {
        /// <summary>
        ///     Minimum permitted message length.
        /// </summary>
        private const int MinMessageLength = 6;

        /// <summary>
        ///     Key used to identify Orvibo messages.
        /// </summary>
        private static readonly byte[] Key = { 0x68, 0x64 };

        /// <summary>
        ///     Parses the specified data to an inbound Orvibo message.
        /// </summary>
        /// <param name="data">Data to parse.</param>
        /// <returns>Parsed inbound message.</returns>
        public IInboundMessage Parse(byte[] data)
        {
            if (data == null)
            {
                throw new MessageException("Error parsing message: data is null.");
            }

            ValidateLength(data);
            ValidateKey(data);

            return CreateMessage(data);
        }

        /// <summary>
        ///     Creates an inbound message based on the message command.
        /// </summary>
        /// <param name="data">Data to parse.</param>
        /// <returns>Created inbound message.</returns>
        private static IInboundMessage CreateMessage(byte[] data)
        {
            var command = data.Skip(4).Take(2).ToArray();
            var upper = command.First();
            var lower = command.Last();

            if (upper == 0x71 && lower == 0x61)
            {
                return new InboundDiscoveryMessage(data);
            }

            if (upper == 0x71 && lower == 0x68)
            {
                return new InboundRediscoveryMessage(data);
            }

            if (upper == 0x63 && lower == 0x6C)
            {
                return new InboundSubscribeMessage(data);
            }

            if (upper == 0x73 && lower == 0x66)
            {
                return new InboundStateChangeMessage(data);
            }

            throw new MessageException("Error parsing message: unknown command.");
        }

        /// <summary>
        ///     Validates the message data, ensuring it begins with the key used to identify Orvibo messages.
        /// </summary>
        /// <param name="data">Message data to validate.</param>
        private static void ValidateKey(byte[] data)
        {
            try
            {
                if (data.IndexOfSequence(Key).First() != 0)
                {
                    throw new InvalidOperationException();
                }
            }
            catch (InvalidOperationException)
            {
                throw new MessageException("Error parsing message: Invalid key.");
            }
        }

        /// <summary>
        ///     Validates the message data, ensuring its length matches the minimum permitted message length.
        /// </summary>
        /// <param name="data">Message data to validate.</param>
        private static void ValidateLength(byte[] data)
        {
            if (data.Length < MinMessageLength)
            {
                throw new MessageException($"Error parsing message: minimum length {MinMessageLength}, actual length {data.Length}.");
            }
        }
    }
}
