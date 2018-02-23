using MQTTClient.Logging;
using System;
using System.Threading.Tasks;
using IoT.Services.Contracts.Messaging;

namespace MQTTClient.Mqtt
{
    /// <summary>
    /// Provides a set of services for Mqtt
    /// </summary>
    internal class MqttService
    {
        private MqttClient client;

        private string[] topics = new string[2];
        private byte[] qos = new byte[2];

        /// <summary>
        /// Creates a new instance of <see cref="MqttService"/>
        /// </summary>
        public MqttService()
        {
            client = new MqttClient("davidber.ddns.net");
            client.Connect(Guid.NewGuid().ToString(), "client", "client");
            topics[0] = "testtopic/devices";
            topics[1] = "testtopic/receive";
            qos[0] = 1;
            qos[1] = 1;
            client.Subscribe(topics, qos);

            client.OnMqttMsgPublishReceived += OnMqttMsgPublishReceived;
        }

        private void OnMqttMsgPublishReceived(object sender, MqttMessageEventArgs e)
        {
            Logger.Info($"New message: Message Type: {e.Message.Payload.PayloadType.ToString()} Payload: {e.Message.Payload.PayloadText}.");
        }

        /// <summary>
        /// Publishes a message to a given topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="message">The message payload.</param>
        /// <returns></returns>
        public async Task Publish(string topic, MqttMessage message)
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
    }
}
