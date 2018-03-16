using IoT.Services.Contracts.Eventing;
using IoT.Services.EventBus.RabbitMQ;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Services.EventBus
{
    /// <summary>
    /// The event bus. <seealso cref="IDisposable"/>
    /// </summary>
    public class EventBusService : IEventBus, IDisposable
    {
        const string brokerName = "IotEventBus";

        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly IEventBusSubscriptionsManager subsManager;
        private IModel consumerChannel;
        private string queueName;
        private int retryCount;

        public bool IsConnected => persistentConnection.IsConnected;

        internal EventBusService(string queueName, IRabbitMQPersistentConnection connection, IEventBusSubscriptionsManager subscriptionsManager)
        {
            persistentConnection = connection;
            subsManager = subscriptionsManager;
            this.queueName = queueName;
            consumerChannel = CreateConsumerChannel();
            retryCount = 5;
            subsManager.OnEventRemoved += SubsManagerOnEventRemoved;
        }

        private void SubsManagerOnEventRemoved(object sender, string eventName)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            using (var channel = persistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: queueName,
                    exchange: brokerName,
                    routingKey: eventName);

                if (subsManager.IsEmpty)
                {
                    queueName = string.Empty;
                    consumerChannel.Close();
                }
            }
        }

        /// <summary>
        /// Publishes an event.
        /// </summary>
        /// <param name="event">The event.</param>
        public void Publish(IntegrationEventBase @event)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            //var policy = RetryPolicy.Handle<BrokerUnreachableException>()
            //    .Or<SocketException>()
            //    .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            //    {
            //        //TODO: Implement Logger.
            //        Console.WriteLine("Failed publishing Retrying...");
            //    });

            using (var channel = persistentConnection.CreateModel())
            {
                var eventName = @event.GetType()
                    .Name;

                channel.ExchangeDeclare(exchange: brokerName,
                                    type: "direct");

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: brokerName,
                                     routingKey: eventName,
                                     basicProperties: null,
                                     body: body);
            }
        }

        /// <summary>
        /// Subscribes an event handler for an event.
        /// </summary>
        /// <typeparam name="T">The event.</typeparam>
        /// <param name="action">The event handler.</param>
        public void Subscribe<T>(IIntegrationEventHandler eventHandler)
            where T : IntegrationEventBase
        {
            var eventName = subsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);
            subsManager.AddSubscription<T>(eventHandler);
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!persistentConnection.IsConnected)
                {
                    persistentConnection.TryConnect();
                }

                using (var channel = persistentConnection.CreateModel())
                {
                    channel.QueueBind(queue: queueName,
                                      exchange: brokerName,
                                      routingKey: eventName);
                }
            }
        }



        /// <summary>
        /// Unsubscribes an event handler for an event.
        /// </summary>
        /// <typeparam name="T">The event.</typeparam>
        public void Unsubscribe<T>()
            where T : IntegrationEventBase
        {
            subsManager.RemoveSubscription<T>();
        }

        /// <summary>
        /// The dispose method.
        /// </summary>
        public void Dispose()
        {
            if (consumerChannel != null)
            {
                consumerChannel.Dispose();
            }

            subsManager.Clear();
        }

        private IModel CreateConsumerChannel()
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var channel = persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: brokerName,
                                 type: "direct");

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body);

                 await ProcessEvent(eventName, message);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                consumerChannel.Dispose();
                consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (subsManager.HasSubscriptionsForEvent(eventName))
            {
                var eventHandler = subsManager.GetHandlerForEvent(eventName);
                var eventType = subsManager.GetEventTypeByName(eventName);
                var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                await Task.Run(() =>
                {
                    //eventHandler?.Invoke((IntegrationEventBase)integrationEvent);
                    var handler = eventHandler as IIntegrationEventHandler<IntegrationEventBase>;
                    handler.Handle((IntegrationEventBase)integrationEvent);
                });
            }
        }
    }
}
