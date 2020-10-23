using SimpleCQRSApp.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SimpleCQRSApp.Tests.Utility
{
    public abstract class GenericAggregateBaseTest<TAggregate, TAggregateId>
        where TAggregate : AggregateBase<TAggregateId>, IAggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        protected void AssertSingleUncommittedEventOfType<TEvent>(TAggregate aggregate)
            where TEvent : IDomainEvent<TAggregateId>
        {
            var uncommittedEvents = GetUncommittedEventsOf(aggregate);

            Assert.Single(uncommittedEvents);
            Assert.IsType<TEvent>(uncommittedEvents.First());
        }

        protected void AssertSingleUncommittedEvent<TEvent>(TAggregate aggregate, Action<TEvent> assertions)
            where TEvent : IDomainEvent<TAggregateId>
        {
            AssertSingleUncommittedEventOfType<TEvent>(aggregate);
            assertions((TEvent)((IEventSourcingAggregate<TAggregateId>)aggregate).GetUncommittedEvents().Single());
        }

        protected void ClearUncommittedEvents(TAggregate aggregate)
        {
            ((IEventSourcingAggregate<TAggregateId>)aggregate).ClearUncommittedEvents();
        }

        protected IEnumerable<IDomainEvent<TAggregateId>> GetUncommittedEventsOf(TAggregate aggregate)
        {
            return ((IEventSourcingAggregate<TAggregateId>)aggregate).GetUncommittedEvents();
        }
    }
}
