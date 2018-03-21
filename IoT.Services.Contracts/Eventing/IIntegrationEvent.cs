using System;

namespace IoT.Services.Contracts.Eventing
{
    public interface IIntegrationEvent
    {
        /// <summary>
        /// The event unique id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// The creation date of the event.
        /// </summary>
        DateTime CreationDate { get; }
    }
}