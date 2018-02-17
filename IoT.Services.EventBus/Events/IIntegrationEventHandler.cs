using System.Threading.Tasks;

namespace IoT.Services.EventBus.Events
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler 
        where TIntegrationEvent: IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event = null);
    }

    public interface IIntegrationEventHandler
    {
    }
}
