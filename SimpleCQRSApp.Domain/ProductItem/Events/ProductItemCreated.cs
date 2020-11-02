using SimpleCQRSApp.Infrastructure.Core;

namespace SimpleCQRSApp.Domain.ProductItem
{
    public class ProductItemCreated : DomainEventBase<ProductId>
    {
        ProductItemCreated()
        {
        }

        internal ProductItemCreated(ProductId aggregateId) : base(aggregateId)
        {

        }

        public ProductItemCreated(ProductId aggregateId, long aggregateVersion ) : base(aggregateId, aggregateVersion)
        {

        }


        public override IDomainEvent<ProductId> WithAggregate(ProductId aggregateId, long aggregateVersion)
        {
            return new ProductItemCreated(aggregateId, aggregateVersion);
        }
    }
}
