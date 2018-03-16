using IoT.Services.MqttServices.Logging;
using IoT.Services.Contracts.Messaging;
using Polly;
using System;
using System.Threading.Tasks;
using M2MMessaging = uPLibrary.Networking.M2Mqtt.Messages;
using M2MNetworking = uPLibrary.Networking.M2Mqtt;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace IoT.Services.MqttServices.Mqtt
{
    /// <summary>
    /// A facade for an Mqtt client.
    /// </summary>
    public class MqttClient : IMqttClient
    {
        private M2MNetworking.MqttClient client;

        /// <summary>
        /// Raised when a message is received.
        /// </summary>
        public event EventHandler<MqttMessageEventArgs> OnMqttMsgPublishReceived;

        /// <summary>
        /// Creates a new instance of <see cref="MqttClient"/>
        /// </summary>
        /// <param name="broker">The name or IP address of the broker.</param>
        public MqttClient(string broker)
        {
            client = new M2MNetworking.MqttClient(broker);
            client.MqttMsgPublishReceived += OnClientMqttMsgPublishReceived;
        }

        private void OnClientMqttMsgPublishReceived(object sender, M2MMessaging.MqttMsgPublishEventArgs e)
        {
            try
            {
                if (OnMqttMsgPublishReceived == null) return;
                var msgString = System.Text.Encoding.Default.GetString(e.Message);
                var msg = JsonConvert.DeserializeObject<MqttMessage>(msgString);
                OnMqttMsgPublishReceived.Invoke(sender, new MqttMessageEventArgs { Message = msg });
            }
            catch (Exception ex)
            {

                Logger.Error(ex.Message);
            }

        }

        /// <summary>
        /// Connects the client to an mqtt broker.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void Connect(string clientId, string username, string password)
        {
            // Wait and retry forever, calling an action on each retry with the 
            // current exception and the time to wait
            Policy
              .Handle<Exception>()
              .WaitAndRetryForever(
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timespan) =>
                {
                    Logger.Error("Failed to connect. Retrying.");
                }).Execute(() =>
                {
                    client.Connect(clientId, username, password);
                });
        }

        /// <summary>
        /// Connects the client to an mqtt broker.
        /// </summary>
        public void Connect(string clientId)
        {

            client.Connect(clientId);
        }

        /// <summary>
        /// Publishes a message to a given topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="message">The message payload.</param>
        /// <returns></returns>
        public async Task Publish(string topic, MqttMessagePayload message)
        {
            await Policy.Handle<Exception>()
                 .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, context) =>
                                 {
                                     Logger.Error("Error while publishing. Retrying");
                                 }
                              ).ExecuteAsync(async () =>
                              {
                                  await Task.Run(() =>
                                  {
                                      client.Publish(topic, message.ToByteArray());
                                  });

                              });

        }

        /// <summary>
        /// Subscribes the client to a give set of topics.
        /// </summary>
        /// <param name="topics">The array of topics to be subscribed.</param>
        /// <param name="qos">The array of qos for each topic.</param>
        public void Subscribe(string[] topics, byte[] qos)
        {
            client.Subscribe(topics, qos);
        }

        /// <summary>
        /// Disconnects the client.
        /// </summary>
        public void Disconnect()
        {
            client.Disconnect();
        }
    }

    public class MqttMessageEventArgs : EventArgs
    {
        public MqttMessage Message { get; set; }
    }
}
