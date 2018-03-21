using Newtonsoft.Json;
using System;
using System.Text;

namespace IoT.Services.Contracts.Messaging
{
    /// <summary>
    /// The message contract
    /// </summary>
    public class MqttMessagePayload
    {
        /// <summary>
        /// Creates a new instance of <see cref="MqttMessagePayload"/>
        /// </summary>
        /// <param name="message">the message of the payload.</param>
        public MqttMessagePayload(string message) : this(JsonConvert.DeserializeObject<MqttMessagePayload>(message))
        {           
        }

        [JsonConstructor]
        public MqttMessagePayload()
        {
        }

        private MqttMessagePayload(MqttMessagePayload message)
        {
            MessageType = message.MessageType;
            Key = message.Key;
            Value = message.Value;
            Timestamp = message.Timestamp;
            DeviceId = message.DeviceId;
        }


        /// <summary>
        /// Creates a new instance of <see cref="MqttMessagePayload"/>
        /// </summary>
        /// <param name="payload">the message of the payload.</param>
        public MqttMessagePayload(byte[] payload)
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

        [JsonProperty]
        public string DeviceId { get; set; }


        /// <summary>
        /// Converts the message to a byte array.
        /// </summary>
        /// <returns>The payload as a byte array.</returns>
        public byte[] ToByteArray()
        {
            return Encoding.ASCII.GetBytes(this.Serialize());
        }

        private MqttMessagePayload Deserialize(string message)
        {
            return JsonConvert.DeserializeObject<MqttMessagePayload>(message);
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
