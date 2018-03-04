﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;

namespace IoT.Services.MqttServices.TcpSocket
{ 
    class SignalRService
    {
        public SignalRService()
        {
            RunWebSockets().GetAwaiter().GetResult();
        }
      
        private static async Task RunWebSockets()
        {
            var ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri("http://localhost:4200/chat/ws/negotiate"), CancellationToken.None);

            Console.WriteLine("Connected");

            var sending = Task.Run(async () =>
            {
                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    var bytes = Encoding.UTF8.GetBytes(line);
                    await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, endOfMessage: true, cancellationToken: CancellationToken.None);
                }

                await ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            });

            var receiving = Receiving(ws);

            await Task.WhenAll(sending, receiving);
        }

        private static async Task Receiving(ClientWebSocket ws)
        {
            var buffer = new byte[2048];

            while (true)
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, result.Count));
                }
                else if (result.MessageType == WebSocketMessageType.Binary)
                {
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }

            }
        }
    }
}
