using IoT.Services.Api.Services;
using IoT.Services.Contracts.Eventing;
using IoT.Services.Contracts.Messaging;

namespace IoT.Services.SignalRWebService.Eventing
{
    /// <summary>
    /// Handles the event when a new mesage is received from a device.
    /// </summary>
    internal class MessageReceivedHandler : 
        IntegrationEventBase, IIntegrationEventHandler<NewMqttMessageEvent>
    {
        public MqttMessage Message { get; private set; }
        
        public void Handle(NewMqttMessageEvent @event)
        {
            Message = @event.Message;
            
        }
    }
}
