using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Kledex.Transactions
{
    public class AmbientTransactionService : IAmbientTransactionService
    {
        /// <inheritdoc />
        public async Task ProcessAsync(Func<Task> execute)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await execute();
                    scope.Complete();
                }
                catch (Exception)
                {
                    // TODO: Handle failure
                }
            }
        }

        /// <inheritdoc />
        public void Process(Action execute)
        {
            using (var scope = new TransactionScope(
                 TransactionScopeOption.Required,
                 new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                try
                {
                    execute();
                    scope.Complete();
                }
                catch (Exception)
                {
                    // TODO: Handle failure
                }
            }
        }
    }
}
