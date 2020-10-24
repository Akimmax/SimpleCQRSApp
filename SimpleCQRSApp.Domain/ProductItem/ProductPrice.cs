using System;

namespace SimpleCQRSApp.Domain.ProductItem
{
    public class ProductPrice
    {
        public string Id { get; set; }
        public int ItemId { get; set; }
        public DateTimeOffset Date { get; set; }
        public double CurrentPrice { get; set; }
    }
}
