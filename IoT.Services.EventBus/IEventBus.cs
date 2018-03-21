using IoT.Services.Contracts.Eventing;
using System;

namespace IoT.Services.EventBus
{ 
    public interface IEventBus
    {
        /// <summary>
        /// Publishes an event.
        /// </summary>
        /// <param name="event">The event.</param>
        void Publish(IIntegrationEvent @event);

        /// <summary>
        /// Subscribes an event handler for an event.
        /// </summary>
        /// <typeparam name="T">The event.</typeparam>
        /// <param name="action">The event handler.</param>
        void Subscribe<T>(IIntegrationEventHandler eventHandler)
            where T : IIntegrationEvent;


        /// <summary>
        /// Unsubscribes an event handler for an event.
        /// </summary>
        /// <typeparam name="T">The event.</typeparam>
        void Unsubscribe<T>()
            where T : IIntegrationEvent;

        /// <summary>
        /// The flag indicating whether the connection is established.
        /// </summary>
        bool IsConnected { get; }
    }
}
