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
            Payload = new Payload();
            Payload.PayloadType = PayloadType.Value;
            Payload.PayloadText = payload;
        }

        public MqttMessage()
        {
            Payload = new Payload();
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
            Payload = DeserializePayload(sb.ToString());
        }

        /// <summary>
        /// The message payload.
        /// </summary>
        public Payload Payload { get; set; }

        /// <summary>
        /// The serialized Json payload.
        /// </summary>
        public string SerializedPayload { get; set; }

        /// <summary>
        /// Converts the messag payload to a byte array.
        /// </summary>
        /// <returns>The payload as a byte array.</returns>
        public byte[] PayloadByteArray()
        {            
            return Encoding.ASCII.GetBytes(SerializePayload(Payload));
        }

        private Payload DeserializePayload(string payload)
        {
            return JsonConvert.DeserializeObject<Payload>(payload);
        }

        private string SerializePayload(Payload payload)
        {
            return JsonConvert.SerializeObject(payload);
        }
    }
}
