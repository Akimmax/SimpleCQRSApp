using SimpleCQRSApp.Domain;
using System.Threading.Tasks;

namespace SimpleCQRSApp.Services
{
    public interface IProductWriteService
    {
        Task<string> CreateProductAsync(string newCode, string newImageSource, string newProductUrl, string newDescription,  int newPriceValue);

        Task UpdateProductAsync(string productId, string newCode, string newImageSource, string newProductUrl, string newDescription, int newPriceValue);

        Task ChangeProductPriceAsync(string productId, int newPriceValue);
    }
}
