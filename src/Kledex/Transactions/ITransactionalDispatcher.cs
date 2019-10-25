using Kledex.Commands;
using Kledex.Events;
using System;
using System.Threading.Tasks;

namespace Kledex.Transactions
{
    public interface ITransactionalDispatcher
    {
        Task DispatchAsync(ICommand command, Func<Task> execute);
        Task DispatchAsync(IEvent @event, Func<Task> execute);
        void Dispatch(ICommand command, Action execute);
        void Dispatch(IEvent @event, Action execute);
    }
}
