using System.Collections.Generic;

namespace SimpleCQRSApp.Infrastructure.Core
{
    public interface IEventSourcingAggregate<TAggregateId>
    {
        long Version { get; }
        void ApplyEvent(IDomainEvent<TAggregateId> @event, long version);
        IEnumerable<IDomainEvent<TAggregateId>> GetUncommittedEvents();
        void ClearUncommittedEvents();
    }
    public interface IAggregate<TId>
    {
        TId Id { get; }
    }
}
