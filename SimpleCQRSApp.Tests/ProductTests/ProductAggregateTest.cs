using SimpleCQRSApp.Domain.ProductItem;
using System;
using Xunit;


namespace SimpleCQRSApp.Tests
{
    [Trait("Type", "Unit")]
    public class ProductAggregateTest : GenericAggregateBaseTest<Product, ProductId>
    {
        
        private static readonly ProductId DefaultProductItemId = ProductId.NewProductItemId();

        [Fact]
        public void CreateProductItem_ProductItemCreated()
        {
            var ProductItem = new Product(DefaultProductItemId);

            AssertSingleUncommittedEvent<ProductItemCreated>(ProductItem, @event =>
            {
                Assert.Equal(DefaultProductItemId, @event.AggregateId);
            });
        }

        [Fact]
        public void UpdateProductItem_ProductItemChanged()
        {

            var ProductItem = new Product(DefaultProductItemId);

            string Code = "Code1";
            string ImageSource = "ImageSource1";
            string ProductUrl = "ProductUrl1";
            string Description = "Description1";

            ClearUncommittedEvents(ProductItem);
            ProductItem.ChangeProductItem(Code, ImageSource, ProductUrl, Description);


            AssertSingleUncommittedEvent<ProductItemChanged>(ProductItem, @event =>
            {
                Assert.Equal(Code, @event.Code);
                Assert.Equal(ImageSource, @event.ImageSource);
                Assert.Equal(DefaultProductItemId, @event.AggregateId);
                Assert.Equal(ProductUrl, @event.ProductUrl);
                Assert.Equal(Description, @event.Description);
            });
        }

        [Fact]
        public void UpdatePrice_ProductPriceChanged()
        {
            var ProductItem = new Product(DefaultProductItemId);

            ClearUncommittedEvents(ProductItem);
            string priceId = Guid.NewGuid().ToString();
            int priceValue = 5;
            ProductItem.ChangeProductPrice(priceId, priceValue);
            AssertSingleUncommittedEvent<ProductPriceChanged>(ProductItem, @event =>
            {
                Assert.Equal(priceId, @event.PriceId);
                Assert.Equal(priceValue, @event.Quantity);
            });
        }
    }
}
