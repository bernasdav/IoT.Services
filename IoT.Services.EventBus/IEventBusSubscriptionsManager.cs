using IoT.Services.Contracts.Eventing;
using System;
using System.Collections.Generic;
using static IoT.Services.EventBus.InMemoryEventBusSubscriptionsManager;

namespace IoT.Services.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;
       
        void AddSubscription<T>(Action<IntegrationEvent> action)
           where T : IntegrationEvent;

        void RemoveSubscription<T>()
             where T : IntegrationEvent;

        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
        bool HasSubscriptionsForEvent(string eventName);
        Type GetEventTypeByName(string eventName);
        void Clear();
        //IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;
        //IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        Action<IntegrationEvent> GetHandlerForEvent(string eventName);
        string GetEventKey<T>();
    }
}