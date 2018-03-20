using IoT.Services.Api.Channels;
using IoT.Services.Contracts.Eventing;
using IoT.Services.Contracts.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace IoT.Services.SignalRWebService.Eventing
{
    /// <summary>
    /// Handles the event when a new mesage is received from a device.
    /// </summary>
    internal class MessageReceivedHandler :  IIntegrationEventHandler<NewMqttMessageEvent>
    {
        IHubContext<SignalRHub> hubContext;

        public MessageReceivedHandler(IHubContext<SignalRHub> hubContext)
        {
            this.hubContext = hubContext;
        }
      
        public void Handle(NewMqttMessageEvent @event)
        {
            hubContext.Clients.All.InvokeAsync("newMessage", @event.Message);
        }
    }
}
