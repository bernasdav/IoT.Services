using IoT.Services.EventBus.Events;
using System;
using System.Collections.Generic;
using static IoT.Services.EventBus.InMemoryEventBusSubscriptionsManager;

namespace IoT.Services.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;
        //void AddDynamicSubscription<TH>(string eventName)
        //   where TH : IDynamicIntegrationEventHandler;

        void AddSubscription<T>(Action action)
           where T : IntegrationEvent;

        void RemoveSubscription<T>()
             where T : IntegrationEvent;
        //void RemoveDynamicSubscription<TH>(string eventName)
        //    where TH : IDynamicIntegrationEventHandler;

        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
        bool HasSubscriptionsForEvent(string eventName);
        Type GetEventTypeByName(string eventName);
        void Clear();
        IEnumerable<Action> GetHandlersForEvent<T>() where T : IntegrationEvent;
        IEnumerable<Action> GetHandlersForEvent(string eventName);
        string GetEventKey<T>();
    }
}