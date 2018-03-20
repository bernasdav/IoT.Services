using System;
using System.Threading.Tasks;
using IoT.Services.Contracts.Messaging;
using IoT.Services.MqttServices.Eventing;
using IoT.Services.MqttServices.Logging;
using System.Collections.Generic;
using System.Threading;

namespace IoT.Services.MqttServices.Mqtt
{
    /// <summary>
    /// Provides a set of services for Mqtt
    /// </summary>
    public class MqttService
    {
        private IMqttClient client;

        private string[] topics = new string[2];
        private byte[] qos = new byte[2];

        /// <summary>
        /// The event raised when a message is received.
        /// </summary>
        public event EventHandler<MqttMessageEventArgs> OnMqttMessageReceived;

        /// <summary>
        /// Creates a new instance of <see cref="MqttService"/>
        /// </summary>
        public MqttService(IMqttClient mqttClient)
        {
            client = mqttClient; //new MqttClient("davidber.ddns.net");
            client.Connect(Guid.NewGuid().ToString(), "client", "client");
            //topics[0] = "iot/devices";
            //qos[0] = 1;
            //client.Subscribe(topics, qos);

            client.OnMqttMsgPublishReceived += OnMqttMsgPublishReceived;
        }

        public void Subscribe(string[] topics, byte[] qos)
        {
            client.Subscribe(topics, qos);
        }

        private void OnMqttMsgPublishReceived(object sender, MqttMessageEventArgs e)
        {
            Logger.Info($"New message: {e.Message}.");
            //todo: Notify GUI here through signalR(?).           
            OnMqttMessageReceived?.Invoke(this, e);
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
                       new MqttMessagePayload { Key = "Key", Value = "Value", MessageType = MessageType.DeviceValues, Timestamp = DateTime.Now },
                       new MqttMessagePayload { Key = "Key-1", Value = "Value-1", MessageType = MessageType.DeviceValues, Timestamp = DateTime.Now }
                   }
                        }
                    };
                    OnMqttMsgPublishReceived(this, eventArgs);

                    Thread.Sleep(4000);
                }
            });
        }
    }
}
