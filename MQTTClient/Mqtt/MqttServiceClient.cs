using MQTTClient.Logging;
using System;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTTClient.Mqtt
{
    internal class MqttServiceClient
    {
        private MqttClient client;

        private string[] topics = new string[2];
        private byte[] qos = new byte[2];

        public MqttServiceClient()
        {
            client = new MqttClient("188.98.203.28");
            client.Connect(Guid.NewGuid().ToString(), "client", "client");
            topics[0] = "testtopic/devices";
            topics[1] = "testtopic/receive";
            qos[0] = 1;
            qos[1] = 1;
            client.Subscribe(topics, qos);

            client.MqttMsgPublishReceived += OnMqttMsgPublishReceived;
        }

        private void OnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
           foreach(var c in e.Message)
            {
                sb.Append((char)c);
            }
            Logger.Info(sb.ToString());
        }

        public async Task Publish(string topic, string message)
        {
            await Task.Run(() =>
            {
                byte[] msg = Encoding.ASCII.GetBytes(message);
                client.Publish(topic, msg);
            });
        }

        public void StopService()
        {
            Logger.Info("Shutting down mqtt client");        
            client.Disconnect();
        }
    }
}
