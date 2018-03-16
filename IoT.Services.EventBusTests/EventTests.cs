using Microsoft.VisualStudio.TestTools.UnitTesting;
using IoT.Services.EventBus;
using IoT.Services.EventBus.RabbitMQ;
using Moq;

namespace IoT.Services.EventBusTests
{
    [TestClass]
    public class EventTests
    {
        [TestMethod]
        public void PublishAndReceiveEventSucceed()
        {
            //var connectionMock = new Mock<IRabbitMQPersistentConnection>();
            //var eventBus = new EventBusService(connectionMock.Object);
        }
    }
}
