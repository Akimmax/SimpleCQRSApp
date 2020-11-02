using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleCQRSApp.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleCQRSApp.Services;

namespace SimpleCQRSApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductReadService cartReader;
        private readonly IProductWriteService cartWriter;


        public ProductController(IProductReadService cartReader, IProductWriteService cartWriter )
        {
            this.cartReader = cartReader;
            this.cartWriter = cartWriter;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>>   GetAll()
        {
            var items = await cartReader.FindAllAsync(c => true);
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ProductDto> Get(string id)
        {
            var item = await cartReader.GetByIdAsync(id);
            return item;
        }


        [HttpPost]
        public async Task<string> Post(ProductDto product)
        {
            var id = await cartWriter.CreateProductAsync(product.Code, product.ImageSource, product.ProductUrl, product.Description, product.Price);
            return id;
        }


        [HttpPut]
        public async Task<string> Update(ProductDto product)
        {
            await cartWriter.UpdateProductAsync(product.Id, product.Code, product.ImageSource, product.ProductUrl, product.Description, product.Price);

            return product.Id;
        }
    }
   
}