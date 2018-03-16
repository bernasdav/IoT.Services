using Microsoft.VisualStudio.TestTools.UnitTesting;
using IoT.Services.MqttServices.Mqtt;
using IoT.Services.Contracts.Messaging;
using Moq;
using System;
using System.Collections.Generic;

namespace IoT.Services.MqttServices.Tests
{
    [TestClass]
    public class MqttTest
    {
        [TestMethod]
        public void MessageReceiveSucceed()
        {
            var clientMock = new Mock<IMqttClient>();
            var service = new MqttService(clientMock.Object);
            Dictionary<string, object> values = new Dictionary<string, object>();

            service.OnMqttMessageReceived += (e, a) => 
            {
                foreach (var payload in a.Message.Messages)
                {
                    values.Add(payload.Key, payload.Value);
                }
            };

           clientMock.Raise(m => m.OnMqttMsgPublishReceived += null, new MqttMessageEventArgs
           {
               Message = new MqttMessage
               {
                   Messages = new List<MqttMessagePayload>
                   {
                       new MqttMessagePayload { Key = "Key", Value = "Value", MessageType = MessageType.DeviceValues, Timestamp = DateTime.Now }
                   }
               }
           });

            Assert.IsTrue(values.ContainsKey("Key"));
            Assert.IsTrue(values.ContainsValue("Value"));

        }      
    }
}
