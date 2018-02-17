using IoT.Services.EventBus.Events;
using MQTTClient.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Services.MqttServices.Events
{
    class NewMessageEventHandler : IntegrationEventHandler, IIntegrationEventHandler<NewMessageEvent>
    {
        public Task Handle(NewMessageEvent @event)
        {
            return Task.Run(() =>
            {
                Logger.Info($"Processing event: {@event.Message}");
            });
        }
    }
}
