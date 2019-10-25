using System;
using System.Threading.Tasks;

namespace Kledex.Transactions
{
    public interface IAmbientTransactionService
    {
        Task ProcessAsync(Func<Task> execute);
        void Process(Action execute);       
    }
}
