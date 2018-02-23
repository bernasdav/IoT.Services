using RabbitMQ.Client;
using System;

namespace IoT.Services.EventBus.RabbitMQ
{
    /// <summary>
    /// Represents a RabbitMQ connection.
    /// </summary>
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {

        /// <summary>
        /// The connected flag.
        /// </summary>
        /// <value><c>True</c> if connected otherwise <c>False</c>.</value>
        bool IsConnected { get; }

        /// <summary>
        /// Tries to connect to the RabbitMQ server.
        /// </summary>
        /// <returns><c>True</c> if connected otherwise <c>False</c>.</returns>
        bool TryConnect();

        /// <summary>
        /// Creates a connection model.
        /// </summary>
        /// <returns>The <see cref="IModel"/> object</returns>
        IModel CreateModel();
    }
}
