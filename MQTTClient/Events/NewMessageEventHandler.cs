using MQTTClient.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IoT.Services.Contracts.Eventing;
using MQTTClient.Mqtt;

namespace IoT.Services.MqttServices.Events
{
    class NewMessageEventHandler : IntegrationEvent, IIntegrationEventHandler<NewMessageEvent>
    {
        private MqttService mqttService;

        public NewMessageEventHandler(MqttService service)
        {
            mqttService = service;
        }

        public async void Handle(NewMessageEvent @event)
        {

            Logger.Info($"Processing event: {@event.Message.Payload}");
            await mqttService.Publish("testtopic/receive", @event.Message);
        }
    }
}
