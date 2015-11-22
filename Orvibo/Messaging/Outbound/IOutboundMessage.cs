namespace Orvibo.Messaging.Outbound
{
    /// <summary>
    ///     Represents an outbound Orvibo message.
    /// </summary>
    internal interface IOutboundMessage
    {
        /// <summary>
        ///     Gets the raw payload data.
        /// </summary>
        /// <returns>Payload data.</returns>
        byte[] GetPayload();
    }
}
