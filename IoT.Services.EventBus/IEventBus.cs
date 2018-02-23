using IoT.Services.Contracts.Eventing;
using System;

namespace IoT.Services.EventBus
{ 
    interface IEventBus
    {
        void Publish(IntegrationEvent @event);

        void Subscribe<T>(Action<IntegrationEvent> action)
            where T : IntegrationEvent;

         void Unsubscribe<T>()
            where T : IntegrationEvent;
    }
}
