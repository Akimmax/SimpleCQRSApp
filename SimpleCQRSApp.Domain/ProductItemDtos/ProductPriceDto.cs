using SimpleCQRSApp.Infrastructure.Read;
using System;

namespace SimpleCQRSApp.Domain
{
    public class ProductPriceDto : IReadEntity
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public DateTimeOffset Date { get; set; }
        public double CurrentPrice { get; set; }

    }
}
