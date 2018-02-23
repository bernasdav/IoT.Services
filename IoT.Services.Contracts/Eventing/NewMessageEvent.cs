using System;
using System.Collections.Generic;
using System.Text;
using IoT.Services.Contracts.Eventing;
using IoT.Services.Contracts.Messaging;

namespace IoT.Services.MqttServices.Events
{
    public class NewMessageEvent : IntegrationEvent
    {
        public MqttMessage Message { get; set; }
    }
}
