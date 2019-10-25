using Kledex.Commands;
using Kledex.Events;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Options = Kledex.Configuration.Options;

namespace Kledex.Transactions
{
    public class TransactionalDispatcher : ITransactionalDispatcher
    {
        private readonly IAmbientTransactionService _ambientTransactionService;
        private readonly Options _options;

        private bool UseAmbientTransaction(bool? requestOption) => requestOption ?? _options.UseAmbientTransactions;

        public TransactionalDispatcher(IAmbientTransactionService ambientTransactionService, IOptions<Options> options)
        {
            _ambientTransactionService = ambientTransactionService;
            _options = options.Value;
        }

        public Task DispatchAsync(ICommand command, Func<Task> execute)
        {
            return DispatchAsync(command.UseAmbientTransaction, () => execute());
        }

        public Task DispatchAsync(IEvent @event, Func<Task> execute)
        {
            return DispatchAsync(@event.UseAmbientTransaction, () => execute());
        }

        private Task DispatchAsync(bool? requestOption, Func<Task> execute)
        {
            return UseAmbientTransaction(requestOption) ? _ambientTransactionService.ProcessAsync(() => execute()) : execute();
        }

        public void Dispatch(ICommand command, Action execute)
        {
            Dispatch(command.UseAmbientTransaction, () => execute());
        }

        public void Dispatch(IEvent @event, Action execute)
        {
            Dispatch(@event.UseAmbientTransaction, () => execute());
        }

        private void Dispatch(bool? requestOption, Action execute)
        {
            if (UseAmbientTransaction(requestOption))
            {
                _ambientTransactionService.Process(() => execute());
            }
            else
            {
                execute();
            }
        }
    }
}
