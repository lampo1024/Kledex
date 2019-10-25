using System.Threading.Tasks;
using Kledex.Bus;
using Kledex.Commands;
using Kledex.Domain;
using Kledex.Events;
using Kledex.Queries;
using Kledex.Transactions;

namespace Kledex
{
    /// <inheritdoc />
    /// <summary>
    /// Dispatcher
    /// </summary>
    /// <seealso cref="T:Kledex.IDispatcher" />
    public class Dispatcher : IDispatcher
    {
        private readonly ICommandSender _commandSender;
        private readonly IDomainCommandSender _domainCommandSender;
        private readonly IEventPublisher _eventPublisher;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IBusMessageDispatcher _busMessageDispatcher;
        private readonly ITransactionalDispatcher _transactionalDispatcher;

        public Dispatcher(ICommandSender commandSender,
            IDomainCommandSender domainCommandSender,
            IEventPublisher eventPublisher, 
            IQueryProcessor queryProcessor, 
            IBusMessageDispatcher busMessageDispatcher,
            ITransactionalDispatcher transactionalDispatcher)
        {
            _commandSender = commandSender;
            _domainCommandSender = domainCommandSender;
            _eventPublisher = eventPublisher;
            _queryProcessor = queryProcessor;
            _busMessageDispatcher = busMessageDispatcher;
            _transactionalDispatcher = transactionalDispatcher;
        }

        /// <inheritdoc />
        public Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            return _transactionalDispatcher.DispatchAsync(command, () => _commandSender.SendAsync(command));
        }

        /// <inheritdoc />
        public Task SendAsync<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            return _transactionalDispatcher.DispatchAsync(command, () => _domainCommandSender.SendAsync<TCommand, TAggregate>(command));
        }

        /// <inheritdoc />
        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return _transactionalDispatcher.DispatchAsync(@event, () => _eventPublisher.PublishAsync(@event));
        }

        /// <inheritdoc />
        public Task<TResult> GetResultAsync<TResult>(IQuery<TResult> query)
        {
            return _queryProcessor.ProcessAsync(query);
        }

        /// <inheritdoc />
        public Task DispatchBusMessageAsync<TMessage>(TMessage message) where TMessage : IBusMessage
        {
            return _busMessageDispatcher.DispatchAsync(message);
        }

        /// <inheritdoc />
        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            _transactionalDispatcher.Dispatch(command, () => _commandSender.Send(command));
        }

        /// <inheritdoc />
        public void Send<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            _transactionalDispatcher.Dispatch(command, () => _domainCommandSender.Send<TCommand, TAggregate>(command));
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _transactionalDispatcher.Dispatch(@event, () => _eventPublisher.Publish(@event));
        }

        /// <inheritdoc />
        public TResult GetResult<TResult>(IQuery<TResult> query)
        {
            return _queryProcessor.Process(query);
        }
    }
}