using IoT.Services.Contracts.Eventing;
using System;
using System.Collections.Generic;
using static IoT.Services.EventBus.InMemoryEventBusSubscriptionsManager;

namespace IoT.Services.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        /// <summary>
        /// Chekcs whether the the sibscriptions manager is empty.
        /// </summary>
        /// <value>
        /// Returns <c>True</c> if empty otherwise <c>False</c>.
        /// </value>
        bool IsEmpty { get; }

        /// <summary>
        /// Raised when an event is removed.
        /// </summary>
        event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// Adds a subscription. <seealso cref="IntegrationEventBase"/>
        /// </summary>
        /// <typeparam name="T">The integration event.</typeparam>
        /// <param name="action">The event handler delegate.</param>
        void AddSubscription<T>(IIntegrationEventHandler eventHandler)
           where T : IntegrationEventBase;

        /// <summary>
        /// Removes a subscriptions. <seealso cref="IntegrationEventBase"/>
        /// </summary>
        /// <typeparam name="T">The integration event.</typeparam>
        void RemoveSubscription<T>()
             where T : IntegrationEventBase;

        /// <summary>
        /// Checks if there is an event handler for the given event.
        /// </summary>
        /// <typeparam name="T">The integration event.</typeparam>
        /// <returns><c>True</c> if the event handler exists otherwise <c>False</c></returns>
        bool HasSubscriptionsForEvent<T>() where T : IntegrationEventBase;

        /// <summary>
        /// Checks if there is an event handler for the given event.
        /// </summary>
        /// <param name="eventName">The vent name.</param>
        /// <returns><c>True</c> if the event handler exists otherwise <c>False</c></returns>
        bool HasSubscriptionsForEvent(string eventName);

        /// <summary>
        /// Gets the event type by name.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>The <c>Type</c> of the event.</returns>
        Type GetEventTypeByName(string eventName);

        /// <summary>
        /// Clears the subscription manager.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the handler of an event.
        /// </summary>
        /// <param name="eventName">The event name.</param>
        /// <returns>The event handler delegate.</returns>
        IIntegrationEventHandler GetHandlerForEvent(string eventName);

        /// <summary>
        /// Gets the key for the event.
        /// </summary>
        /// <typeparam name="T">The integration event.</typeparam>
        /// <returns>The event key.</returns>
        string GetEventKey<T>();
    }
}