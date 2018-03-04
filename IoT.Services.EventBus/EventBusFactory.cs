using IoT.Services.EventBus.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Services.EventBus
{
    /// <summary>
    /// A factory for event bus services.
    /// </summary>
    public class EventBusFactory
    {
        public static EventBusService GetEventBus()
        {
            var subscriptionManager = new InMemoryEventBusSubscriptionsManager();
            var rabbitMQConnection = new DefaultRabbitMQPersistentConnection(new ConnectionFactory { HostName = "davidber.ddns.net", UserName = "client", Password = "client" });
            return new EventBusService(new Guid().ToString(), rabbitMQConnection, subscriptionManager);
        }
    }
}
