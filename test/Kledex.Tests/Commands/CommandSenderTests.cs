﻿using System;
using System.Collections.Generic;
using Kledex.Commands;
using Kledex.Dependencies;
using Kledex.Domain;
using Kledex.Events;
using Kledex.Tests.Fakes;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Options = Kledex.Configuration.Options;

namespace Kledex.Tests.Domain
{
    [TestFixture]
    public class CommandSenderTests
    {
        private ICommandSender _sut;

        private Mock<IHandlerResolver> _handlerResolver;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IDomainStore> _domainStore;
        private Mock<IEventFactory> _eventFactory;

        private Mock<ICommandHandler<CreateSomething>> _commandHandler;
        private Mock<ICommandHandler<CreateAggregate>> _domainCommandHandler;
        private Mock<IOptions<Options>> _optionsMock;

        private CreateSomething _createSomething;
        private SomethingCreated _somethingCreated;
        private SomethingCreated _somethingCreatedConcrete;
        private IEnumerable<IEvent> _events;

        private CreateAggregate _createAggregate;
        private AggregateCreated _aggregateCreated;
        private AggregateCreated _aggregateCreatedConcrete;
        private Aggregate _aggregate;

        private CommandResponse _commandResponse;
        private CommandResponse _domainCommandResponse;

        [SetUp]
        public void SetUp()
        {
            _createSomething = new CreateSomething();
            _somethingCreated = new SomethingCreated();
            _somethingCreatedConcrete = new SomethingCreated();
            _events = new List<IEvent> { _somethingCreated };

            _createAggregate = new CreateAggregate();
            _aggregateCreatedConcrete = new AggregateCreated();
            _aggregate = new Aggregate();
            _aggregateCreated = (AggregateCreated)_aggregate.Events[0];

            _commandResponse = new CommandResponse { Events = _events };
            _domainCommandResponse = new CommandResponse { Events = _aggregate.Events };

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher
                .Setup(x => x.Publish(_aggregateCreatedConcrete));

            _domainStore = new Mock<IDomainStore>();
            _domainStore
                .Setup(x => x.Save(_aggregate.GetType(), _createAggregate.AggregateRootId, _createAggregate, new List<IDomainEvent>() { _aggregateCreated }));

            _eventFactory = new Mock<IEventFactory>();
            _eventFactory
                .Setup(x => x.CreateConcreteEvent(_somethingCreated))
                .Returns(_somethingCreatedConcrete);
            _eventFactory
                .Setup(x => x.CreateConcreteEvent(_aggregateCreated))
                .Returns(_aggregateCreatedConcrete);

            _commandHandler = new Mock<ICommandHandler<CreateSomething>>();
            _commandHandler
                .Setup(x => x.Handle(_createSomething))
                .Returns(_commandResponse);

            _domainCommandHandler = new Mock<ICommandHandler<CreateAggregate>>();
            _domainCommandHandler
                .Setup(x => x.Handle(_createAggregate))
                .Returns(_domainCommandResponse);

            _handlerResolver = new Mock<IHandlerResolver>();
            _handlerResolver
                .Setup(x => x.ResolveCommandHandler(_createSomething, typeof(ICommandHandler<>)))
                .Returns(_commandHandler.Object);
            _handlerResolver
                .Setup(x => x.ResolveCommandHandler(_createAggregate, typeof(ICommandHandler<>)))
                .Returns(_domainCommandHandler.Object);

            _optionsMock = new Mock<IOptions<Options>>();
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options());

            _sut = new CommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _domainStore.Object,
                _optionsMock.Object);
        }

        [Test]
        public void Send_ThrowsException_WhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.Throws<ArgumentNullException>(() => _sut.Send(_createAggregate));
        }

        [Test]
        public void Send_HandlesCommand()
        {
            _sut.Send(_createSomething);
            _commandHandler.Verify(x => x.Handle(_createSomething), Times.Once);
        }

        [Test]
        public void Send_HandlesDomainCommand()
        {
            _sut.Send(_createAggregate);
            _domainCommandHandler.Verify(x => x.Handle(_createAggregate), Times.Once);
        }

        [Test]
        public void Send_SavesEvents()
        {
            _sut.Send(_createAggregate);
            _domainStore.Verify(x => x.Save(_aggregate.GetType(), _createAggregate.AggregateRootId, _createAggregate, new List<IDomainEvent>() { _aggregateCreated }), Times.Once);
        }

        [Test]
        public void Send_PublishesEvents()
        {
            _sut.Send(_createAggregate);
            _eventPublisher.Verify(x => x.Publish(_aggregateCreatedConcrete ), Times.Once);
        }

        [Test]
        public void Send_NotPublishesEvents_WhenSetInOptions()
        {
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options { PublishEvents = false });

            _sut = new CommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _domainStore.Object,
                _optionsMock.Object);

            _sut.Send(_createAggregate);
            _eventPublisher.Verify(x => x.Publish(_aggregateCreatedConcrete), Times.Never);
        }

        [Test]
        public void Send_NotPublishesEvents_WhenSetInCommand()
        {
            _createAggregate.PublishEvents = false;
            _sut.Send(_createAggregate);
            _eventPublisher.Verify(x => x.Publish(_aggregateCreatedConcrete), Times.Never);
        }
    }
}
