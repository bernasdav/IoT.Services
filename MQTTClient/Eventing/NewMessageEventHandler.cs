using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IoT.Services.Contracts.Eventing;
using IoT.Services.MqttServices.Mqtt;
using IoT.Services.MqttServices.Logging;

namespace IoT.Services.MqttServices.Eventing
{
    /// <summary>
    /// The evet handler for a message directed to a device.
    /// </summary>
    class NewMessageEventHandler : IIntegrationEventHandler
    {
        private MqttService mqttService;

        public NewMessageEventHandler(MqttService service)
        {
            mqttService = service;
        }

        public  void Handle(IIntegrationEvent @event)
        {
            //Logger.Info($"Processing event: {@event.Message.Serialize()}");
            //await mqttService.Publish("testtopic/receive", @event.Message);
        }
    }
}
