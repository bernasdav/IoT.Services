using System;
using System.Threading.Tasks;
using IoT.Services.Contracts.Messaging;

namespace MQTTClient.Mqtt
{
    /// <summary>
    /// A facade for an Mqtt client.
    /// </summary>
    internal interface IMqttClient
    {
        /// <summary>
        /// Raised when a message is received.
        /// </summary>
        event EventHandler<MqttMessageEventArgs> OnMqttMsgPublishReceived;

        /// <summary>
        /// Connects the client to an mqtt broker.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        void Connect(string clientId, string username, string password);

        /// <summary>
        /// Connects the client to an mqtt broker.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        void Connect(string clientId);

        /// <summary>
        /// Subscribes the client to a give set of topics.
        /// </summary>
        /// <param name="topics">The array of topics to be subscribed.</param>
        /// <param name="qos">The array of qos for each topic.</param>
        void Subscribe(string[] topics, byte[] qos);

        /// <summary>
        /// Publishes a message to a given topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="message">The message payload.</param>
        /// <returns></returns>
        Task Publish(string topic, MqttMessage message);

        /// <summary>
        /// Disconnects the client.
        /// </summary>
        void Disconnect();

    }
}
