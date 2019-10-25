﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Sample.NoEventSourcing.Data;
using Kledex.Sample.NoEventSourcing.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.NoEventSourcing.Domain.Commands.Handlers
{
    public class WithdrawProductHandler : IDomainCommandHandlerAsync<WithdrawProduct>
    {
        private readonly SampleDbContext _dbContext;

        public WithdrawProductHandler(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<IDomainEvent>> HandleAsync(WithdrawProduct command)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == command.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {command.AggregateRootId}");
            }

            product.Withdraw();

            await _dbContext.SaveChangesAsync();

            return new List<IDomainEvent>
            {
                new ProductWithdrew
                {
                    AggregateRootId = product.Id
                }
            };
        }
    }
}
