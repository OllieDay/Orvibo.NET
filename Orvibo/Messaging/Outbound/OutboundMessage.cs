using System;
using System.Linq;

namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Provides the base implementation of an outbound Orvibo message.
    /// </summary>
    internal abstract class OutboundMessage : IOutboundMessage
    {
        /// <summary>
        ///     Key used to identify Orvibo messages.
        /// </summary>
        private static readonly byte[] Key = { 0x68, 0x64 };

        /// <summary>
        ///     Command for the message type.
        /// </summary>
        protected abstract byte[] Command { get; }

        /// <summary>
        ///     Gets the raw payload data.
        /// </summary>
        /// <returns>Payload data.</returns>
        public byte[] GetPayload()
        {
            return Combine(Key, GetLengthBytes(), Command, GetData());
        }

        /// <summary>
        ///     Combines a sequence of byte[].
        /// </summary>
        /// <param name="sequences">Sequence of byte[] to combine.</param>
        /// <returns>Combined byte[] sequence.</returns>
        protected static byte[] Combine(params byte[][] sequences)
        {
            return sequences.SelectMany(x => x).ToArray();
        }

        /// <summary>
        ///     Gets the data for the message type.
        /// </summary>
        /// <returns>Data for the messae type.</returns>
        protected virtual byte[] GetData()
        {
            return new byte[0];
        }

        /// <summary>
        ///     Gets the message length.
        /// </summary>
        /// <returns>Message length.</returns>
        private ushort GetLength()
        {
            // 2 is equal to GetLengthBytes().Length.
            return (ushort) (Key.Length + 2 + Command.Length + GetData().Length);
        }

        /// <summary>
        ///     Gets the message length in bytes.
        /// </summary>
        /// <returns>Message length in bytes.</returns>
        private byte[] GetLengthBytes()
        {
            return BitConverter.GetBytes(GetLength()).Reverse().ToArray();
        }
    }
}
