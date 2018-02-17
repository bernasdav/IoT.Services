using System.Threading.Tasks;

namespace IoT.Services.EventBus.Events
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
