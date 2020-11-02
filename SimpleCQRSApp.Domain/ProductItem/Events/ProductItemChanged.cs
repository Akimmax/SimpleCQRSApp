using SimpleCQRSApp.Infrastructure.Core;

namespace SimpleCQRSApp.Domain.ProductItem
{
    public class ProductItemChanged : DomainEventBase<ProductId>
    {
        ProductItemChanged()
        {
        }

        internal ProductItemChanged(string newCode, string newImageSource, string newProductUrl, string newDescription) : base()
        {

            Code = newCode;
            ImageSource = newImageSource;
            ProductUrl = newProductUrl;
            Description = newDescription;
        }

        private ProductItemChanged(ProductId aggregateId, long aggregateVersion, string newCode, string newImageSource, string newProductUrl, string newDescription) : base(aggregateId, aggregateVersion)
        {
            Code = newCode;
            ImageSource = newImageSource;
            ProductUrl = newProductUrl;
            Description = newDescription;
        }

        public string Code { get; set; }
        public string ImageSource { get; set; }
        public string ProductUrl { get; set; }
        public string Description { get; set; }

        public override IDomainEvent<ProductId> WithAggregate(ProductId aggregateId, long aggregateVersion)
        {
            return new ProductItemChanged(aggregateId, aggregateVersion, Code, ImageSource, ProductUrl, Description);
        }
    }
}
