using IoT.Services.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IoT.Services.EventBus
{
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {


        private readonly Dictionary<string, Action> handlers;
        private readonly List<Type> eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public InMemoryEventBusSubscriptionsManager()
        {
            handlers = new Dictionary<string, Action>();
            eventTypes = new List<Type>();
        }

        public bool IsEmpty => !handlers.Keys.Any();
        public void Clear() => handlers.Clear();

        public void AddSubscription<T>(Action action)
            where T : IntegrationEvent
        {
            var eventName = GetEventKey<T>();
            DoAddSubscription(action, eventName, isDynamic: false);
            eventTypes.Add(typeof(T));
        }

        private void DoAddSubscription(Action action, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                handlers.Add(eventName, action);
            }

            //if (handlers[eventName].Any(s => s.HandlerType == handlerType))
            //{
            //    throw new ArgumentException(
            //        $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            //}

            //if (isDynamic)
            //{
            //    handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
            //}
            //else
            //{
            //    handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
            //}
        }


        public void RemoveSubscription<T>()
            where T : IntegrationEvent
        {
            var handlerToRemove = FindSubscriptionToRemove<T>();
            var eventName = GetEventKey<T>();
            DoRemoveHandler(eventName);
        }


        private void DoRemoveHandler(string eventName)
        {
            handlers.Remove(eventName);
        }

        public IEnumerable<Action> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var key = GetEventKey<T>();
            return GetHandlersForEvent(key);
        }
        public IEnumerable<Action> GetHandlersForEvent(string eventName) => new List<Action> { handlers[eventName] };

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            if (handler != null)
            {
                OnEventRemoved(this, eventName);
            }
        }

        private Action FindSubscriptionToRemove<T>()
             where T : IntegrationEvent
        {
            var eventName = GetEventKey<T>();
            return DoFindSubscriptionToRemove(eventName);
        }

        private Action DoFindSubscriptionToRemove(string eventName)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }

            return handlers[eventName];

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
    }
}
