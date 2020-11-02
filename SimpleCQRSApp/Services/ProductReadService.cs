using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SimpleCQRSApp.Domain;
using SimpleCQRSApp.Infrastructure.Read;

namespace SimpleCQRSApp.Services
{
    public class ProductReadService : IProductReadService
    {

        private IRepository<ProductDto> productItemRepository;
        private IRepository<ProductPriceDto> productItemPriceRepository;

 
        public ProductReadService(IRepository<ProductDto> productItemRepository, IRepository<ProductPriceDto> productItemPriceRepository)
        {
            this.productItemRepository = productItemRepository;
            this.productItemPriceRepository = productItemPriceRepository;
        }

        public async Task<IEnumerable<ProductDto>> FindAllAsync(Expression<Func<ProductDto, bool>> predicate)
        {
            return await productItemRepository.FindAllAsync(predicate);
        }

        public async Task<ProductDto> GetByIdAsync(string id)
        {
            return await productItemRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ProductDto>> GetItemsOfAsync(string Code)
        {
            return await productItemRepository.FindAllAsync(x => x.Code == Code);
        }
    }
}
