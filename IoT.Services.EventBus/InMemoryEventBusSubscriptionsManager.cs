using IoT.Services.Contracts.Eventing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IoT.Services.EventBus
{
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {


        private readonly Dictionary<string, Action<IntegrationEvent>> handlers;
        private readonly List<Type> eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public InMemoryEventBusSubscriptionsManager()
        {
            handlers = new Dictionary<string, Action<IntegrationEvent>>();
            eventTypes = new List<Type>();
        }

        public bool IsEmpty => !handlers.Keys.Any();
        public void Clear() => handlers.Clear();

        public void AddSubscription<T>(Action<IntegrationEvent> action)
            where T : IntegrationEvent
        {
            var eventName = GetEventKey<T>();
            DoAddSubscription(action, eventName, isDynamic: false);
            eventTypes.Add(typeof(T));
        }

        private void DoAddSubscription(Action<IntegrationEvent> action, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                handlers.Add(eventName, action);
            }

            //if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            //{
            //    throw new ArgumentException(
            //        $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            //}

            //if (isDynamic)
            //{
            //    _handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
            //}
            //else
            //{
            //    _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
            //}
        }

        public void RemoveSubscription<T>()
            where T : IntegrationEvent
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

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return HasSubscriptionsForEvent(key);
        }
        public bool HasSubscriptionsForEvent(string eventName) => handlers.ContainsKey(eventName);

        public Type GetEventTypeByName(string eventName) => eventTypes.SingleOrDefault(t => t.Name == eventName);

        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }

        public Action<IntegrationEvent> GetHandlerForEvent(string eventName)
        {
            return handlers[eventName];
        }
    }
}
