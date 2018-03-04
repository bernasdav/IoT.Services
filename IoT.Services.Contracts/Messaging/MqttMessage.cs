using Newtonsoft.Json;
using System;
using System.Text;

namespace IoT.Services.Contracts.Messaging
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
        }

        [JsonConstructor]
        private MqttMessage()
        {

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
        }

        /// <summary>
        /// Gets or sets the message type. <seealso cref="MessageType"/>
        /// </summary>
        [JsonProperty]
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [JsonProperty]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the message value.
        /// </summary>
        [JsonProperty]
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the message.
        /// </summary>
        [JsonProperty]
        public DateTime Timestamp { get; set; }


        /// <summary>
        /// Converts the message to a byte array.
        /// </summary>
        /// <returns>The payload as a byte array.</returns>
        public byte[] ToByteArray()
        {
            return Encoding.ASCII.GetBytes(this.Serialize());
        }

        /// <summary>
        /// Deserializes a message.
        /// </summary>
        /// <param name="payload">The message payload.</param>
        /// <returns>a new instace of <see cref="MqttMessage"/></returns>
        public static MqttMessage Deserialize(string payload)
        {
            return JsonConvert.DeserializeObject<MqttMessage>(payload);
        }

        /// <summary>
        /// Serializes the current message to a Json string
        /// </summary>
        /// <returns>The message as Json string.</returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
