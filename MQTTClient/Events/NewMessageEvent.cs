using System;
using System.Collections.Generic;
using System.Text;
using IoT.Services.EventBus.Events;

namespace IoT.Services.MqttServices.Events
{
    class NewMessageEvent : IntegrationEvent
    {
        public string Message => "Hallo";
    }
}
