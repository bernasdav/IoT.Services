using System;
using System.Collections.Generic;
using System.Text;
using Polly;
using MQTTClient.Logging;

using Quobject.SocketIoClientDotNet.Client;
using System.Threading.Tasks;

namespace IoT.Services.MqttServices.TcpSocket
{
    public class NotificationSocket
    {
        Socket socket;

        public NotificationSocket()
        {
            socket = IO.Socket("http://127.0.0.1:8888");

            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Logger.Info($"Connected to server.");
            });
        }

        public void SendData(string dataToSend)
        {
            Task.Run(() =>
            {
                try
                {
                    socket.Emit("data", dataToSend);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }
            });
        }
    }
}