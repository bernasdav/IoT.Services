using System;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTClient
{
    class Program
    {
        private static Mqtt.MqttServiceClient client;
        private static DockerLifeTimeHandler dockerLifeTimeHandler;

        static void Main(string[] args)
        {            
            dockerLifeTimeHandler = new DockerLifeTimeHandler();
            dockerLifeTimeHandler.Starting += DockerLifeTimeHandler_Starting;
            dockerLifeTimeHandler.Stopping += DockerLifeTimeHandler_Stopping;

            dockerLifeTimeHandler.Start();
        }

        private static void DockerLifeTimeHandler_Stopping(object sender, EventArgs e)
        {
            client.StopService();
        }

        private static void SimulateSend()
        {
            Task.Run(async () => {
                int nr = 1;
                while (true)
                {
                    await client.Publish("testtopic/receive", nr.ToString());
                    Logging.Logger.Info("Sending");
                    nr++;
                    if (nr == 4) nr = 1;
                    Thread.Sleep(50);
                }                
            });
        }

        private static void DockerLifeTimeHandler_Starting(object sender, EventArgs e)
        {
            client = new Mqtt.MqttServiceClient();
            SimulateSend();
        }
    }
}
