using IoT.Services.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Services.MqttServices.Events
{
    class NewMessageEventHandler : IIntegrationEventHandler<NewMessageEvent>
    {
        public Task Handle(NewMessageEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
