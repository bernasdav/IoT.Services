using System;
using System.Text;

namespace IoT.Services.Contracts
{
    /// <summary>
    /// The message contract
    /// </summary>
    public class MqttMessage
    {
        /// <summary>
        /// Creates a new instance of <see cref="MqttMessage"/>
        /// </summary>
        /// <param name="payload">the message of the payload.</param>
        public MqttMessage(string payload)
        {
            Payload = payload;
        }

        /// <summary>
        /// Creates a new instance of <see cref="MqttMessage"/>
        /// </summary>
        /// <param name="payload">the message of the payload.</param>
        public MqttMessage(byte[] payload)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in payload)
            {
                sb.Append((char)c);
            }
            Payload = sb.ToString();
        }

        /// <summary>
        /// The message payload.
        /// </summary>
        public string Payload { get; private set; }

        /// <summary>
        /// Converts the messag payload to a byte array.
        /// </summary>
        /// <returns>The payload as a byte array.</returns>
        public byte[] ToByteArray()
        {
            return Encoding.ASCII.GetBytes(Payload);
        }
    }
}
