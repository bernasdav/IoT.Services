using Microsoft.VisualStudio.TestTools.UnitTesting;
using IoT.Services.MqttServices.Mqtt;
using IoT.Services.Contracts.Messaging;
using Moq;
using System;
using System.Collections.Generic;
using IoT.Services.EventBus;
using IoT.Services.Contracts.Eventing;

namespace IoT.Services.MqttServices.Tests
{
    [TestClass]
    public class MqttTest
    {
        [TestMethod]
        public void MessageReceiveSucceed()
        {
            var clientMock = new Mock<IMqttClient>();
            var eventBusMock = new Mock<IEventBus>();
            var eventMock = new Mock<IIntegrationEvent>();
            var service = new MqttService(clientMock.Object, eventBusMock.Object);
            Dictionary<string, object> values = new Dictionary<string, object>();

            clientMock.Raise(m => m.OnMqttMsgPublishReceived += null, new MqttMessageEventArgs
            {
                Message = new MqttMessage
                {
                    Messages = new List<MqttMessagePayload>
                   {
                       new MqttMessagePayload { Key = "Key", Value = "Value", MessageType = MessageType.DeviceInfo, Timestamp = DateTime.Now },
                       new MqttMessagePayload { Key = "Key", Value = "Value", MessageType = MessageType.DeviceInfo, Timestamp = DateTime.Now }
                   }
                }
            });
            string[] topics = new string[1] { "Value" };
            byte[] qoss = new byte[1] { 1 };

            clientMock.Verify(m => m.Subscribe(topics, qoss), Times.Exactly(2));
        }
    }
}
