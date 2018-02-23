using IoT.Services.Contracts.Eventing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IoT.Services.EventBus
{
    /// <summary>
    /// Manages the subscriptions for the event bus.
    /// </summary>
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {


        private readonly Dictionary<string, Action<IntegrationEventBase>> handlers;
        private readonly List<Type> eventTypes;

        /// <summary>
        /// Raised when an event is removed.
        /// </summary>
        public event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// Creates a new instance of <see cref="InMemoryEventBusSubscriptionsManager"/>
        /// </summary>
        public InMemoryEventBusSubscriptionsManager()
        {
            handlers = new Dictionary<string, Action<IntegrationEventBase>>();
            eventTypes = new List<Type>();
        }

        /// <summary>
        /// Chekcs whether the the sibscriptions manager is empty.
        /// </summary>
        /// <value>
        /// Returns <c>True</c> if empty otherwise <c>False</c>.
        /// </value>
        public bool IsEmpty => !handlers.Keys.Any();

        /// <summary>
        /// Clears the subscription manager.
        /// </summary>
        public void Clear() => handlers.Clear();

        /// <summary>
        /// Adds a subscription. <seealso cref="IntegrationEventBase"/>
        /// </summary>
        /// <typeparam name="T">The integration event.</typeparam>
        /// <param name="action">The event handler delegate.</param>
        public void AddSubscription<T>(Action<IntegrationEventBase> action)
            where T : IntegrationEventBase
        {
            var eventName = GetEventKey<T>();
            DoAddSubscription(action, eventName, isDynamic: false);
            eventTypes.Add(typeof(T));
        }

        private void DoAddSubscription(Action<IntegrationEventBase> action, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                handlers.Add(eventName, action);
            }           
        }

        /// <summary>
        /// Removes a subscriptions. <seealso cref="IntegrationEventBase"/>
        /// </summary>
        /// <typeparam name="T">The integration event.</typeparam>
        public void RemoveSubscription<T>()
            where T : IntegrationEventBase
        {
            var eventName = GetEventKey<T>();
            DoRemoveHandler(eventName);
        }


        private void DoRemoveHandler(string eventName)
        {
            if (handlers.ContainsKey(eventName))
            {
                handlers.Remove(eventName);
            }
        }

        /// <summary>
        /// Checks if there is an event handler for the given event.
        /// </summary>
        /// <typeparam name="T">The integration event.</typeparam>
        /// <returns><c>True</c> if the event handler exists otherwise <c>False</c></returns>
        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEventBase
        {
            var key = GetEventKey<T>();
            return HasSubscriptionsForEvent(key);
        }

        /// <summary>
        /// Checks if there is an event handler for the given event.
        /// </summary>
        /// <param name="eventName">The vent name.</param>
        /// <returns><c>True</c> if the event handler exists otherwise <c>False</c></returns>
        public bool HasSubscriptionsForEvent(string eventName) => handlers.ContainsKey(eventName);

        /// <summary>
        /// Gets the event type by name.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>The <c>Type</c> of the event.</returns>
        public Type GetEventTypeByName(string eventName) => eventTypes.SingleOrDefault(t => t.Name == eventName);

        /// <summary>
        /// Gets the key for the event.
        /// </summary>
        /// <typeparam name="T">The integration event.</typeparam>
        /// <returns>The event key.</returns>
        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }

        /// <summary>
        /// Gets the handler of an event.
        /// </summary>
        /// <param name="eventName">The event name.</param>
        /// <returns>The event handler delegate.</returns>
        public Action<IntegrationEventBase> GetHandlerForEvent(string eventName)
        {
            return handlers[eventName];
        }
    }
}
