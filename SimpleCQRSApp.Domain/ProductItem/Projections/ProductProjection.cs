using System.Threading.Tasks;
using SimpleCQRSApp.Infrastructure.Read;
using SimpleCQRSApp.Domain;
using SimpleCQRSApp.Domain.ProductItem;
using System;

namespace SimpleCQRSApp.Infrastructure.Handlers
{
    public class ProductProjection : IDomainEventHandler<ProductId, ProductItemCreated>,
        IDomainEventHandler<ProductId, ProductItemChanged>, 
        IDomainEventHandler<ProductId, ProductPriceChanged>
    {

        private readonly IRepository<ProductDto> cartRepository;
        private readonly IRepository<ProductPriceDto> cartItemRepository;

        public ProductProjection(
            IRepository<ProductDto> cartRepository,
            IRepository<ProductPriceDto> cartItemRepository
            )
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
        }

        public async Task HandleAsync(ProductItemCreated @event)
        {
            await cartRepository.InsertAsync(new ProductDto
            {
                Id = @event.AggregateId.IdAsString()
            });
        }

        public async Task HandleAsync(ProductItemChanged @event)
        {
            var cart = await cartRepository.GetByIdAsync(@event.AggregateId.IdAsString());
            cart.Code = @event.Code;
            cart.ImageSource = @event.ImageSource;
            cart.ProductUrl = @event.ProductUrl;
            cart.Description = @event.Description;

            await cartRepository.UpdateAsync(cart);
        }

        public async Task HandleAsync(ProductPriceChanged @event)
        {
            var cart = await cartRepository.GetByIdAsync(@event.AggregateId.IdAsString());


            var priceItem = new ProductPriceDto()
            {
                CurrentPrice = @event.Quantity,
                Id = @event.PriceId,
                Date = DateTimeOffset.Now,
                ItemId = cart.Id
            };

            cart.Price = @event.Quantity;
            await cartRepository.UpdateAsync(cart);
            await cartItemRepository.InsertAsync(priceItem);

        }

    }
}
