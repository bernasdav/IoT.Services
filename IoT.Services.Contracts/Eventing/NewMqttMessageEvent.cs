using System;
using System.Collections.Generic;
using System.Text;
using IoT.Services.Contracts.Eventing;
using IoT.Services.Contracts.Messaging;

namespace IoT.Services.Contracts.Eventing
{
    /// <summary>
    /// An event raised when a new mqtt message is received.
    /// </summary>
    public class NewMqttMessageEvent : IIntegrationEvent
    {
        public NewMqttMessageEvent(MqttMessage message)
        {
            Message = message;
        }

        /// <summary>
        /// The <see cref="MqttMessage"/>
        /// </summary>
        public MqttMessage Message { get; private set; }

        public Guid Id => new Guid();

        public DateTime CreationDate => DateTime.Now;
    }
}
