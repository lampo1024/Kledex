using System;
using System.Threading.Tasks;

namespace Kledex.Store.EF.Transactions
{
    public interface ITransactionService
    {
        Task ProcessAsync(Func<Task> execute);
    }

    public class TransactionService : ITransactionService
    {
        private readonly IDomainDbContextFactory _dbContextFactory;

        public TransactionService(IDomainDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task ProcessAsync(Func<Task> execute)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    await execute();
                }
            }
        }
    }
}
