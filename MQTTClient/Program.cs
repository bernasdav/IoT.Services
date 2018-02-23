using System;
using System.Threading;
using System.Threading.Tasks;
using IoT.Services.EventBus;
using IoT.Services.MqttServices.Events;
using Autofac;
using System.Collections.Generic;
using IoT.Services.Contracts.Eventing;
using IoT.Services.Contracts.Messaging;

namespace MQTTClient
{
    class Program
    {
        private static Mqtt.MqttService client;
        private static DockerLifeTimeHandler dockerLifeTimeHandler;
        private static EventBusService eventBus;

        static void Main(string[] args)
        {
            dockerLifeTimeHandler = new DockerLifeTimeHandler();
            dockerLifeTimeHandler.Starting += DockerLifeTimeHandler_Starting;
            dockerLifeTimeHandler.Stopping += DockerLifeTimeHandler_Stopping;

            dockerLifeTimeHandler.Start();
        }

        private static void DockerLifeTimeHandler_Stopping(object sender, EventArgs e)
        {
            client.StopService();
        }

        private static void SimulateSend()
        {
            Task.Run(async () =>
            {
                int nr = 1;
                while (true)
                {
                    var msg = new IoT.Services.Contracts.Messaging.MqttMessage(1.ToString());
                    await client.Publish("testtopic/receive", msg);
                    Logging.Logger.Info("Sending");
                    nr++;
                    if (nr == 4) nr = 1;
                    Thread.Sleep(1000);
                }
            });
        }

        private static void DockerLifeTimeHandler_Starting(object sender, EventArgs e)
        {
            client = new Mqtt.MqttService();
            eventBus = new EventBusService();
            Action<IntegrationEvent> eventHandlerDelegate = (@event) =>
            {
                var handler = new NewMessageEventHandler(client);
                handler.Handle((NewMessageEvent)@event);
            };
            eventBus.Subscribe<NewMessageEvent>(eventHandlerDelegate);
            SimulateEvent();
        }


        private static void SimulateEvent()
        {
            var @event = new NewMessageEvent();
            @event.Message = new MqttMessage();
            @event.Message.Payload.PayloadText = "1";
            eventBus.Publish(@event);
        }
    }
}
