using SimpleCQRSApp.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCQRSApp.Domain.ProductItem
{

    public class ProductId : IAggregateId
    {
        private const string IdAsStringPrefix = "ProductItem-";

        public Guid Id { get; private set; }

        private ProductId(Guid id)
        {
            Id = id;
        }

        public ProductId(string id)
        {
            Id = Guid.Parse(id.StartsWith(IdAsStringPrefix) ? id.Substring(IdAsStringPrefix.Length) : id);
        }

        public override string ToString()
        {
            return IdAsString();
        }

        public override bool Equals(object obj)
        {
            return obj is ProductId && Equals(Id, ((ProductId)obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static ProductId NewProductItemId()
        {
            return new ProductId(Guid.NewGuid());
        }

        public string IdAsString()
        {
            return $"{IdAsStringPrefix}{Id.ToString()}";
        }
    }
}
