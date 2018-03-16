using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace IoT.Services.Contracts.Messaging
{
    /// <summary>
    /// The messag class containing the payload-
    /// </summary>
    public class MqttMessage
    {
        [JsonConstructor]
        public MqttMessage()
        {
        }

        /// <summary>
        /// The list of message payloads.
        /// </summary>
        [JsonProperty]
        public IEnumerable<MqttMessagePayload> Messages { get; set; }
    }
}
