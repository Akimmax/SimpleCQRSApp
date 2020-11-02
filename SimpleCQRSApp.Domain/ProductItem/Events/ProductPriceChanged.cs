using SimpleCQRSApp.Domain;
using SimpleCQRSApp.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCQRSApp.Domain.ProductItem
{
    public class ProductPriceChanged : DomainEventBase<ProductId>
    {
        public ProductPriceChanged()
        {
        }

        internal ProductPriceChanged(string priceId, int quantity) : base()
        {
            PriceId = priceId;
            Quantity = quantity;
        }

        public ProductPriceChanged(ProductId aggregateId, long aggregateVersion, string priceId, int quantity) : base(aggregateId, aggregateVersion)
        {
            PriceId = priceId;
            Quantity = quantity;
        }

        public string PriceId { get;  set; }

        public int Quantity { get;  set; }

        public override IDomainEvent<ProductId> WithAggregate(ProductId aggregateId, long aggregateVersion)
        {
            return new ProductPriceChanged(aggregateId, aggregateVersion, PriceId, Quantity);
        }
    }
}
