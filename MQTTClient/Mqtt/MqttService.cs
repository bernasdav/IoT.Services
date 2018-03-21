using System;
using System.Threading.Tasks;
using IoT.Services.Contracts.Messaging;
using IoT.Services.Contracts.Eventing;
using IoT.Services.MqttServices.Logging;
using System.Collections.Generic;
using System.Threading;
using IoT.Services.EventBus;

namespace IoT.Services.MqttServices.Mqtt
{
    /// <summary>
    /// Provides a set of services for Mqtt
    /// </summary>
    public class MqttService
    {
        private IMqttClient client;
        private IEventBus eventBus;
        private List<string> subscriptions;

        /// <summary>
        /// Creates a new instance of <see cref="MqttService"/>
        /// </summary>
        public MqttService(IMqttClient mqttClient, IEventBus eventBus)
        {
            client = mqttClient; //new MqttClient("davidber.ddns.net");
            client.Connect(Guid.NewGuid().ToString(), "client", "client");

            string[] topics = new string[1];
            byte[] qos = new byte[1];
            topics[0] = "iot/devices";
            qos[0] = 1;
            client.Subscribe(topics, qos);

            this.eventBus = eventBus;
            subscriptions = new List<string>();
            client.OnMqttMsgPublishReceived += OnMqttMsgPublishReceived;
        }

        private void OnMqttMsgPublishReceived(object sender, MqttMessageEventArgs e)
        {
            foreach (var msg in e.Message.Messages)
            {
                switch (msg.MessageType)
                {
                    case MessageType.DeviceInfo:
                        AddSubscription(msg.Value.ToString(), 1);
                        Logger.Info($"Subscribing topic {msg.Value}");
                        break;
                    case MessageType.DeviceValues:
                        Logger.Info($"Sending message {msg.Value}");
                        SendEvent(e.Message);
                        break;
                    default:
                        Logger.Error($"Could not process message {msg.MessageType}. Key: {msg.Key}, Value: {msg.Value}");
                        break;
                }

                Logger.Info($"New message: Key: {msg.Key} Value:{msg.Value}.");
            }
        }

        private void SendEvent(MqttMessage message)
        {
            var @event = new NewMqttMessageEvent(message);
            eventBus.Publish(@event);
        }

        private void AddSubscription(string topic, byte qos)
        {
            if (subscriptions.Contains(topic))
            {
                Logger.Warn($"Topic {topic} already exists!");
                return;
            }
            string[] topics = new string[1] { topic };
            byte[] qoss = new byte[1] { qos };
            client.Subscribe(topics, qoss);
        }

        /// <summary>
        /// Publishes a message to a given topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="message">The message payload.</param>
        /// <returns></returns>
        public async Task Publish(string topic, MqttMessagePayload message)
        {
            await client.Publish(topic, message);
        }

        /// <summary>
        /// Stops the mqtt client.
        /// </summary>
        public void StopService()
        {
            Logger.Info("Shutting down mqtt client");
            client.Disconnect();
        }


        public void SimulateReceive()
        {

            Task.Run(() =>
            {
                while (true)
                {
                    var eventArgs = new MqttMessageEventArgs
                    {
                        Message = new MqttMessage
                        {
                            Messages = new List<MqttMessagePayload>
                   {
                       new MqttMessagePayload { Key = "topic", Value = "/livingRoom/temperature", MessageType = MessageType.DeviceInfo, Timestamp = DateTime.Now },
                       new MqttMessagePayload { Key = "topic", Value = "/kitchen/temperatur", MessageType = MessageType.DeviceInfo, Timestamp = DateTime.Now }
                   }
                        }
                    };
                    OnMqttMsgPublishReceived(this, eventArgs);

                    Thread.Sleep(10000);
                }
            });
        }
    }
}
