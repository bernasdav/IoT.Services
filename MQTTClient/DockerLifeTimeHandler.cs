using MQTTClient.Logging;
using System;
using System.Runtime.Loader;
using System.Threading;

namespace MQTTClient
{
    /// <summary>
    /// Handles the lifetime of the application inside the docker container.
    /// </summary>
    public class DockerLifeTimeHandler
    {
        public event EventHandler Starting;
        public event EventHandler Stopping;
        ManualResetEventSlim ended = new ManualResetEventSlim();

        public DockerLifeTimeHandler()
        {
            AssemblyLoadContext.Default.Unloading += OnUnloading;
        }

        protected void OnSarting(object sender, EventArgs e)
        {
            if (Starting == null) return;
            Starting(sender, e);
            Logger.Info("Starting Application..");
            ended.Wait();
        }       

        public void Start()
        {
            OnSarting(this,null);
        }

        private void OnUnloading(AssemblyLoadContext obj)
        {
            Logger.Info("Stopping application..");
            if (Stopping == null) return;
            Stopping(this, null);
            Logger.Info("Stopped..");
            ended.Set();
        }
    }
}
