using System;
using System.Collections.Generic;
using SimpleCQRSApp.Infrastructure.Core;


namespace SimpleCQRSApp.Domain.ProductItem
{
    public class Product : AggregateBase<ProductId>
    {
        public const int ProductQuantityThreshold = 50;

        private Product()
        {
            PriceHistory = new List<ProductPrice>();
        }


        private string Code { get; set; }
        private string ImageSource { get; set; }
        private string ProductUrl { get; set; }
        private string Description { get; set; }
        private int Price { get; set; }
        private ICollection<ProductPrice> PriceHistory { get; set; }

        public Product(ProductId productItemId) : this()
        {
            if (productItemId == null)
            {
                throw new ArgumentNullException(nameof(productItemId));
            }

            RaiseEvent(new ProductItemCreated(productItemId));
        }



        public void ChangeProductItem(string newCode, string newImageSource, string newProductUrl, string newDescription)
        {
            //Validation

            RaiseEvent(new ProductItemChanged(
                UpdateIfChanged(Code, newCode),
                UpdateIfChanged(ImageSource, newImageSource),
                UpdateIfChanged(ProductUrl, newProductUrl),
                UpdateIfChanged(Description, newDescription)));
        }


        public void ChangeProductPrice(string priceId, int newPriceValue)
        {
            //Validation

            RaiseEvent(new ProductPriceChanged(priceId, newPriceValue));
        }

        public void Apply(ProductItemCreated ev)
        {
            Id = ev.AggregateId;            
        }

        public void Apply(ProductItemChanged ev)
        {
            Code = ev.Code;
            ImageSource = ev.ImageSource;
            ProductUrl = ev.ProductUrl;
            Description = ev.Description;
        }

        public void Apply(ProductPriceChanged ev)
        {

            var priceItem = 
                new ProductPrice() {
                    CurrentPrice = ev.Quantity,
                    Id = ev.PriceId,
                    Date = DateTimeOffset.Now
                };

            PriceHistory.Add(priceItem);
            Price = ev.Quantity;
        }

        public string UpdateIfChanged (string oldvalue, string newvalue)
        {
            if (newvalue != null)
                return newvalue;
            else
                return oldvalue;
        }
    }
}
