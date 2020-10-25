using SimpleCQRSApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleCQRSApp.Services
{
    public interface IProductReadService
    {
        Task<ProductDto> GetByIdAsync(string id);

        Task<IEnumerable<ProductDto>> FindAllAsync(Expression<Func<ProductDto, bool>> predicate);

        Task<IEnumerable<ProductDto>> GetItemsOfAsync(string cartId);
    }
}
