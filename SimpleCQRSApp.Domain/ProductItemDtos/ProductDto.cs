using System.Collections.Generic;
using SimpleCQRSApp.Infrastructure.Read;

namespace SimpleCQRSApp.Domain
{
    public class ProductDto : IReadEntity
    {
        public string Id { get; set; }

        public string Code { get; set; }
        public string ImageSource { get; set; }
        public string ProductUrl { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
