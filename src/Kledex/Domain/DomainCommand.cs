﻿using System;
using Kledex.Commands;

namespace Kledex.Domain
{
    public abstract class DomainCommand : Command, IDomainCommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AggregateRootId { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public string Source { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public int? ExpectedVersion { get; set; }
        public bool? SaveCommandData { get; set; }
    }
}
