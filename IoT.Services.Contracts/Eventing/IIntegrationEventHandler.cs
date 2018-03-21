using System.Threading.Tasks;

namespace IoT.Services.Contracts.Eventing
{
    
    public interface IIntegrationEventHandler
    {
        void Handle(IIntegrationEvent @event);
    }
}
