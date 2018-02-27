using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNet.SignalR.Hubs;

namespace IoT.Services.MqttServices.TcpSocket
{
    [HubName("NotifierHub")]
    class SignalRService : Hub
    {
        public SignalRService() : base()
        {
            
        }

        public void Send(string message)
        {
            Clients.Others.sendNotification(message);
        }
    }
}
