using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleCQRSApp.Infrastructure.Handlers;
using SimpleCQRSApp.Infrastructure.Persistence;
using SimpleCQRSApp.Infrastructure.PubSub;
using SimpleCQRSApp.Domain.ProductItem;
using SimpleCQRSApp.Infrastructure.Core;
using System;

namespace SimpleCQRSApp.Services
{
    public class ProductWriteService : IProductWriteService
    {
        private readonly IRepository<Product, ProductId> ProductRepository;
        private readonly ITransientDomainEventSubscriber subscriber;
        private readonly IEnumerable<IDomainEventHandler<ProductId, ProductItemCreated>> productCreatedEventHandlers;
        private readonly IEnumerable<IDomainEventHandler<ProductId, ProductItemChanged>> productAddedEventHandlers;
        private readonly IEnumerable<IDomainEventHandler<ProductId, ProductPriceChanged>> productQuantityChangedEventHandlers;

        public ProductWriteService(IRepository<Product, ProductId> ProductRepository,
            ITransientDomainEventSubscriber subscriber,
            IEnumerable<IDomainEventHandler<ProductId, ProductItemCreated>> ProductCreatedEventHandlers,
            IEnumerable<IDomainEventHandler<ProductId, ProductItemChanged>> productAddedEventHandlers,
            IEnumerable<IDomainEventHandler<ProductId, ProductPriceChanged>> productQuantityChangedEventHandlers)
        {
            this.ProductRepository = ProductRepository;
            this.subscriber = subscriber;
            this.productCreatedEventHandlers = ProductCreatedEventHandlers;
            this.productAddedEventHandlers = productAddedEventHandlers;
            this.productQuantityChangedEventHandlers = productQuantityChangedEventHandlers;


            subscriber.Subscribe<ProductItemCreated>(async @event => await HandleAsync(productCreatedEventHandlers, @event));
            subscriber.Subscribe<ProductItemChanged>(async @event => await HandleAsync(productAddedEventHandlers, @event));
            subscriber.Subscribe<ProductPriceChanged>(async @event => await HandleAsync(productQuantityChangedEventHandlers, @event));
        }

        public async Task<string> CreateProductAsync(string newCode, string newImageSource, string newProductUrl, string newDescription, int newPriceValue)
        {

            var id = ProductId.NewProductItemId();
            var Product = new Product(id);
            
            Product.ChangeProductItem(newCode, newImageSource, newProductUrl, newDescription);
            Product.ChangeProductPrice(Guid.NewGuid().ToString(), newPriceValue);
            await ProductRepository.SaveAsync(Product);
            return id.ToString();
        }

        public async Task UpdateProductAsync(string productId, string newCode, string newImageSource, string newProductUrl, string newDescription,  int newPriceValue)
        {
            var Product = await ProductRepository.GetByIdAsync(new ProductId(productId));
          
            Product.ChangeProductItem(newCode, newImageSource, newProductUrl, newDescription);
            Product.ChangeProductPrice(Guid.NewGuid().ToString(), newPriceValue);
            await ProductRepository.SaveAsync(Product);
        }

        public async Task ChangeProductPriceAsync(string ProductId, int newPriceValue)
        {
            var Product = await ProductRepository.GetByIdAsync(new ProductId(ProductId));
            Product.ChangeProductPrice(Guid.NewGuid().ToString(), newPriceValue);
            await ProductRepository.SaveAsync(Product);
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
