using System.Threading.Tasks;

namespace IoT.Services.Contracts.Eventing
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler 
        where TIntegrationEvent: IIntegrationEvent
    {
        void Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
