﻿using System;
using Kledex.Queries;
using Kledex.Sample.EventSourcing.Reporting.Data;

namespace Kledex.Sample.EventSourcing.Reporting.Queries
{
    public class GetProduct : IQuery<ProductEntity>
    {
        public Guid ProductId { get; set; }
    }
}
