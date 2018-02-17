using IoT.Services.EventBus.Events;
using System;

namespace IoT.Services.EventBus
{ 
    interface IEventBus
    {
        void Publish(IntegrationEvent @event);

        void Subscribe<T>(Action action)
            where T : IntegrationEvent;

         void Unsubscribe<T>()
            where T : IntegrationEvent;
    }
}
