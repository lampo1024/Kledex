﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kledex.Domain
{
    public interface IEventStore
    {
        /// <summary>
        /// Saves the event asynchronous.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="event">The event.</param>
        /// <param name="expectedVersion">The expected version.</param>
        /// <returns></returns>
        Task SaveEventAsync<TAggregate>(IDomainEvent @event, int? expectedVersion = null) 
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Gets the events asynchronous.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId);

        /// <summary>
        /// Saves the event.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="event">The event.</param>
        /// <param name="expectedVersion">The expected version.</param>
        void SaveEvent<TAggregate>(IDomainEvent @event, int? expectedVersion = null)
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        IEnumerable<DomainEvent> GetEvents(Guid aggregateId);
    }

    public class DefaultEventStore : IEventStore
    {
        public IEnumerable<DomainEvent> GetEvents(Guid aggregateId)
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }

        public Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }

        public void SaveEvent<TAggregate>(IDomainEvent @event, int? expectedVersion = null) where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }

        public Task SaveEventAsync<TAggregate>(IDomainEvent @event, int? expectedVersion = null) where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }
    }
}
