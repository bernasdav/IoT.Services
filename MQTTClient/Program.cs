using IoT.Services.Contracts.Eventing;
using IoT.Services.EventBus;
using IoT.Services.MqttServices.Eventing;
using IoT.Services.MqttServices.Events;
using IoT.Services.MqttServices.TcpSocket;
using System;

namespace MQTTClient
{
    class Program
    {
        private static Mqtt.MqttService client;
        private static DockerLifeTimeHandler dockerLifeTimeHandler;
        private static NotificationSocket socket;

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
            socket = new NotificationSocket();
            client.OnMqttMessageReceived += OnClientOnMqttMessageReceived;
            ConfigureServices();
            SimulateSend();
        }

        private static void SimulateSend()
        {
            var msg = new IoT.Services.Contracts.Messaging.MqttMessage();
            msg.Payload.PayloadText = "TEst";
            msg.Payload.PayloadType = IoT.Services.Contracts.Messaging.PayloadType.Hello;
            msg.Payload.ValueName = "Key";

            OnClientOnMqttMessageReceived(null, new Mqtt.MqttMessageEventArgs { Message = msg });
        }

        private static void OnClientOnMqttMessageReceived(object sender, Mqtt.MqttMessageEventArgs e)
        {
            socket.SendData(e.Message.SerializedPayload);
        }

        private static void ConfigureServices()
        {
            try
            {
                var eventBus = new EventBusService("MQTTService");
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
