﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Microsoft.AspNetCore.SignalR;

namespace IoT.Services.Api.Services
{

    class SignalRService : Hub
    {
        public SignalRService()
        {
            
        }

        public override Task OnConnectedAsync()
        {
            Send();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public void Send()
        {
            Clients.All.InvokeAsync("data", "Hello");
        }
    }
}