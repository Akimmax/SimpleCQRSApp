using SimpleCQRSApp.Infrastructure.Core;

namespace SimpleCQRSApp.Tests.Utility
{
    public class TestDomainEvent : DomainEventBase<TestAggregateId>
    {
        public override IDomainEvent<TestAggregateId> WithAggregate(TestAggregateId aggregateId, long aggregateVersion)
        {
            return this;
        }
    }
}