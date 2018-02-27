using System;
using System.Collections.Generic;
using System.Text;
using IoT.Services.Contracts.Eventing;
using IoT.Services.Contracts.Messaging;

namespace IoT.Services.MqttServices.Eventing
{
    /// <summary>
    /// An event raised when a new mqtt message is received.
    /// </summary>
    public class NewMqttMessageEvent : IntegrationEventBase
    {
        public NewMqttMessageEvent():base()
        {
        }

        /// <summary>
        /// The <see cref="MqttMessage"/>
        /// </summary>
        public MqttMessage Message { get; set; }
    }
}
