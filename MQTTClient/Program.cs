using IoT.Services.Contracts.Eventing;
using IoT.Services.EventBus;
using IoT.Services.MqttServices.Eventing;
using IoT.Services.MqttServices.Events;
using System;

namespace MQTTClient
{
    class Program
    {
        private static Mqtt.MqttService client;
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

                Logging.Logger.Error(ex.Message);
            }
        }

        private static void DockerLifeTimeHandler_Stopping(object sender, EventArgs e)
        {
            client.StopService();
        }

        private static void DockerLifeTimeHandler_Starting(object sender, EventArgs e)
        {
            client = new Mqtt.MqttService();
            client.OnMqttMessageReceived += OnClientOnMqttMessageReceived;
            ConfigureServices();
            //SimulateSend();
        }

        private static void SimulateSend()
        {
            var msg = new IoT.Services.Contracts.Messaging.MqttMessage("");          

            OnClientOnMqttMessageReceived(null, new Mqtt.MqttMessageEventArgs { Message = msg });
        }

        private static void OnClientOnMqttMessageReceived(object sender, Mqtt.MqttMessageEventArgs e)
        {
            //TODO: Send through event bus (to SignalR)
        }

        private static void ConfigureServices()
        {
            try
            {
                var eventBus = EventBusFactory.GetEventBus();
                Action<IntegrationEventBase> eventHandlerDelegate = (@event) =>
                {
                    var handler = new NewMessageEventHandler(client);
                    handler.Handle((NewMqttMessageEvent)@event);
                };
                eventBus.Subscribe<NewMqttMessageEvent>(eventHandlerDelegate);
            }
            catch (Exception ex)
            {
                Logging.Logger.Error($"Error connecting to Event Bus: {ex.Message}");
            }
        }

    }
}
