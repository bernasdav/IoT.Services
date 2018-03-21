using IoT.Services.MqttServices.Logging;
using IoT.Services.Contracts.Eventing;
using IoT.Services.EventBus;
using IoT.Services.MqttServices.Eventing;
using IoT.Services.MqttServices.Mqtt;
using System;

namespace MQTTClient
{
    class Program
    {
        private static MqttService mqttService;
        private static DockerLifeTimeHandler dockerLifeTimeHandler;
        static void Main(string[] args)
        {
            try
            {
                dockerLifeTimeHandler = new DockerLifeTimeHandler();
                dockerLifeTimeHandler.Starting += DockerLifeTimeHandler_Starting;
                dockerLifeTimeHandler.Stopping += DockerLifeTimeHandler_Stopping;

                dockerLifeTimeHandler.Start();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private static void DockerLifeTimeHandler_Stopping(object sender, EventArgs e)
        {
            mqttService.StopService();
        }

        private static void DockerLifeTimeHandler_Starting(object sender, EventArgs e)
        {
            var client = new MqttClient("localhost"); //new MqttClient("davidber.ddns.net");
            var eventBus = EventBusFactory.GetEventBus();
            mqttService = new MqttService(client, eventBus);

            mqttService.SimulateReceive();
        }        

    }
}
