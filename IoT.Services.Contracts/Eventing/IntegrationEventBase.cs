using System;

namespace IoT.Services.Contracts.Eventing
{
    /// <summary>
    /// The base class for the integration event.
    /// </summary>
    public abstract class IntegrationEventBase : IIntegrationEvent
    {
        /// <summary>
        /// Creates a new instance of <see cref="IntegrationEventBase"/>
        /// </summary>
        public IntegrationEventBase()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        /// <summary>
        /// The event unique id.
        /// </summary>
        public Guid Id  { get; }

        /// <summary>
        /// The creation date of the event.
        /// </summary>
        public DateTime CreationDate { get; }
    }
}
