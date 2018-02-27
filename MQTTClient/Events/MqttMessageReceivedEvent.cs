using IoT.Services.Contracts.Eventing;
using System;
using System.Collections.Generic;
using System.Text;
using IoT.Services.Contracts.Messaging;

namespace IoT.Services.MqttServices.Events
{
    class MqttMessageReceivedEvent : IntegrationEventBase
    {
        public MqttMessage Message { get; set; }
    }
}
