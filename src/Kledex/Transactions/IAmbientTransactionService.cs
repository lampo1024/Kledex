using System;
using System.Threading.Tasks;

namespace Kledex.Transactions
{
    public interface IAmbientTransactionService
    {
        void Process(Action execute);
        Task ProcessAsync(Func<Task> execute);
    }
}
