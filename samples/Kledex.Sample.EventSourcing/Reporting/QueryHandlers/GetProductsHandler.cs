﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kledex.Queries;
using Kledex.Sample.EventSourcing.Domain;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Kledex.Sample.EventSourcing.Reporting.Queries;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.EventSourcing.Reporting.QueryHandlers
{
    public class GetProductsHandler : IQueryHandlerAsync<GetProducts, IList<ProductEntity>>
    {
        private readonly ReportingDbContext _dbContext;

        public GetProductsHandler(ReportingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<ProductEntity>> HandleAsync(GetProducts query)
        {
            return await _dbContext.Products.Where(x => x.Status != ProductStatus.Deleted).ToListAsync();
        }
    }
}
