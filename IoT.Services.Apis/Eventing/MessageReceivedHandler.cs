using IoT.Services.Api.Channels;
using IoT.Services.Contracts.Eventing;
using IoT.Services.Contracts.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace IoT.Services.SignalRWebService.Eventing
{
    /// <summary>
    /// Handles the event when a new mesage is received from a device.
    /// </summary>
    internal class MessageReceivedHandler :  IIntegrationEventHandler
    {
        IHubContext<SignalRHub> hubContext;

        public MessageReceivedHandler(IHubContext<SignalRHub> hubContext)
        {
            this.hubContext = hubContext;
        }
      
        public void Handle(IIntegrationEvent @event)
        {
            hubContext.Clients.All.InvokeAsync("newMessage", ((NewMqttMessageEvent)@event).Message);
        }
    }
}
