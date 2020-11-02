using SimpleCQRSApp.Infrastructure.Persistence;
using SimpleCQRSApp.Infrastructure.Persistence.EventStore;
using SimpleCQRSApp.Infrastructure.PubSub;
using SimpleCQRSApp.Infrastructure.Read;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using MongoDB.Driver;
using SimpleCQRSApp.Infrastructure.Core;
using SimpleCQRSApp.Domain;
using SimpleCQRSApp.Infrastructure.Handlers;
using SimpleCQRSApp.Domain.ProductItem;
using System.Linq;
using SimpleCQRSApp.Tests.Utility;

namespace SimpleCQRSApp.Tests
{

    [Trait("Type", "Unit")]
    public partial class ProductIntegrationTests
    {
        protected IMongoDatabase mongoDB;

        private IRepository<Product, ProductId> sut1;
        private static readonly TestAggregateId DefaultId = new TestAggregateId();
        ITransientDomainEventSubscriber subscriber = new TransientDomainEventPubSub();
        private readonly IEnumerable<IDomainEventHandler<ProductId, ProductItemCreated>> productItemCreatedHandlers;
        private readonly IEnumerable<IDomainEventHandler<ProductId, ProductItemChanged>> productItemChangedHandlers;
        private readonly IEnumerable<IDomainEventHandler<ProductId, ProductPriceChanged>> productItemPriceChangedHandlers;
        private static readonly ProductId DefaultProductItemId = ProductId.NewProductItemId();

        private MongoDBRepository<ProductDto> productItemRepository;
        private MongoDBRepository<ProductPriceDto> productItemPriceRepository;
        private ProductProjection cardEventHandler;


        int priceValue = 5;
        string Code = "TestCode";
        string ImageSource = "TestImageSource";
        string ProductUrl = "TestProductUrl";
        string Description = "TestDescription";
        string updatedDescription = "TestDescription_updated";

        public ProductIntegrationTests()
        {
            sut1 = new EventSourcingRepository<Product, ProductId>(
                new EventStoreEventStore(EventStoreConnection.Create(new Uri("tcp://localhost:1113"))),
                new TransientDomainEventPubSub()
                );

            var client = new MongoClient("mongodb://localhost:27017");
            mongoDB = client.GetDatabase("cqrs1");

            productItemRepository = new MongoDBRepository<ProductDto>(mongoDB);
            productItemPriceRepository = new MongoDBRepository<ProductPriceDto>(mongoDB);

            cardEventHandler = new ProductProjection(productItemRepository, productItemPriceRepository);

            productItemCreatedHandlers = new List<IDomainEventHandler<ProductId, ProductItemCreated>>() { cardEventHandler };
            productItemChangedHandlers = new List<IDomainEventHandler<ProductId, ProductItemChanged>>() { cardEventHandler };
            productItemPriceChangedHandlers = new List<IDomainEventHandler<ProductId, ProductPriceChanged>>() { cardEventHandler };
        }


        //!! Please Ran integration tests only one by one otherwise it leads to EventStoreCommunicationException "Unable to access persistence layer" 
        [Fact]
        public async Task ShouldSaveandRaiseAggregate()
        {
            var ProductItem = new Product(DefaultProductItemId);

            string priceId = Guid.NewGuid().ToString();
            ProductItem.ChangeProductItem(Code, ImageSource, ProductUrl, Description);
            ProductItem.ChangeProductPrice(priceId, priceValue);
            await sut1.SaveAsync(ProductItem);

            var aggregate = await sut1.GetByIdAsync(DefaultProductItemId);
            Assert.Equal(aggregate.Id, DefaultProductItemId);
            
        }

        [Fact]
        //!! Please Ran integration tests only one by one otherwise it leads to EventStoreCommunicationException "Unable to access persistence layer" 
        public async Task ShouldUpdateReadModel()
        {
            subscriber.Subscribe<ProductItemCreated>(async @event => await HandleAsync(productItemCreatedHandlers, @event));
            subscriber.Subscribe<ProductItemChanged>(async @event => await HandleAsync(productItemChangedHandlers, @event));
            subscriber.Subscribe<ProductPriceChanged>(async @event => await HandleAsync(productItemPriceChangedHandlers, @event));

            var ProductItem = new Product(DefaultProductItemId);

            string priceId = Guid.NewGuid().ToString();
            ProductItem.ChangeProductItem(Code, ImageSource, ProductUrl, Description);
            ProductItem.ChangeProductItem(null, null, null, updatedDescription);
            ProductItem.ChangeProductPrice(priceId, priceValue);
            await sut1.SaveAsync(ProductItem);
            
            var productReadModel = await productItemRepository.GetByIdAsync(DefaultProductItemId.ToString());

            var productPriceReadModel = await productItemPriceRepository.FindAllAsync(price => price.ItemId == DefaultProductItemId.ToString());

            Assert.Equal(productReadModel.Id, DefaultProductItemId.ToString());
            Assert.Equal(priceValue, productReadModel.Price);
            Assert.Equal(updatedDescription, productReadModel.Description);
            Assert.Equal(ImageSource, productReadModel.ImageSource);
            Assert.Equal(ProductUrl, productReadModel.ProductUrl);

            Assert.Equal(priceValue, productPriceReadModel.ToList().First().CurrentPrice); 
        }


        public async Task HandleAsync<T>(IEnumerable<IDomainEventHandler<ProductId, T>> handlers, T @event)
            where T : IDomainEvent<ProductId>
        {
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}
