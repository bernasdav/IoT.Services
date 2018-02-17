using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Services.EventBus.Events
{
    public class EventHandlerList<T> : List<T> where T : IIntegrationEventHandler
    {
    }
}
