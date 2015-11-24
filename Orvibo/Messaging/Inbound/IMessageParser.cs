namespace Orvibo.Messaging.Inbound
{
    /// <summary>
    ///     Represents an inbound Orvibo message parser.
    /// </summary>
    internal interface IMessageParser
    {
        /// <summary>
        ///     Parses the specified data to an inbound Orvibo message.
        /// </summary>
        /// <param name="data">Data to parse.</param>
        /// <returns>Parsed inbound message.</returns>
        IInboundMessage Parse(byte[] data);
    }
}
