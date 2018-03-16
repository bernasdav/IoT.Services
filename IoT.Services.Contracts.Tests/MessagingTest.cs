using IoT.Services.Contracts.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;

namespace IoT.Services.Contracts.Tests
{
    [TestClass]
    public class MessagingTest
    {
        [TestMethod]
        public void MessageSerializeSucceeds()
        {
            var message = new MqttMessagePayload()
            {
                Key = "Key",
                Value = "Value",
                MessageType = MessageType.DeviceValues,
                Timestamp = DateTime.Now
            };

            var serializedMessage = message.Serialize();
            JObject obj = JObject.Parse(serializedMessage);

            Assert.AreEqual(obj.Property("Key").Value, message.Key);
            Assert.AreEqual(obj.Property("Value").Value, message.Value.ToString());
            Assert.AreEqual(obj.Property("MessageType").Value, (int)message.MessageType);
            Assert.AreEqual(obj.Property("Timestamp").Value, message.Timestamp);
        }

        [TestMethod]
        public void MessageDeserializeSucceeds()
        {
            dynamic json = new JObject();
            json.Key = "Key";
            json.Value = "Value";
            json.MessageType = "0";
            json.Timestamp = DateTime.Now;

            MqttMessagePayload message = new MqttMessagePayload(json.ToString());

            Assert.AreEqual(json.Key.Value, message.Key);
            Assert.AreEqual(json.Value.Value, message.Value.ToString());
            Assert.AreEqual(json.Timestamp.Value, message.Timestamp);
        }
    }
}
