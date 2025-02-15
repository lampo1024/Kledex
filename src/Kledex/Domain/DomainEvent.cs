﻿using System;
using Kledex.Events;

namespace Kledex.Domain
{
    public abstract class DomainEvent : Event, IDomainEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AggregateRootId { get; set; }
        public Guid CommandId { get; set; }
        public string UserId { get; set; }
        public string Source { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public void Update(IDomainCommand command)
        {
            CommandId = command.Id;
            UserId = command.UserId;
            Source = command.Source;
        }
    }
}
