using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCQRSApp.Tests
{

    [Trait("Type", "Integration")]
    public class MongoDBTest : MongoDBTestBase
    {
        private const string CollectionName = "TestEntity";

        [Fact]
        public async Task CanInsertAnItemInCollection()
        {
            string id = await InsertTestElementWithQuantity(5);
            var test = await mongoDB.GetCollection<TestEntity>(CollectionName)
                .Find(x => x.Id == id)
                .SingleAsync();

            Assert.NotNull(test);
            Assert.Equal(id, test.Id);
            Assert.Equal(5, test.Quantity);
        }

        [Fact]
        public async Task CanUpdateQuantity()
        {
            string id = await InsertTestElementWithQuantity(5);

            var test = await mongoDB.GetCollection<TestEntity>(CollectionName)
                .Find(x => x.Id == id)
                .SingleAsync();

            await mongoDB.GetCollection<TestEntity>(CollectionName)
                .UpdateOneAsync(x => x.Id == id, Builders<TestEntity>.Update.Set(x => x.Quantity, 8));

            test = await mongoDB.GetCollection<TestEntity>(CollectionName)
                .Find(x => x.Id == id)
                .SingleAsync();

            Assert.Equal(8, test.Quantity);
        }

        private async Task<string> InsertTestElementWithQuantity(int quantity)
        {
            string id = Guid.NewGuid().ToString();

            await mongoDB.GetCollection<TestEntity>(CollectionName)
                .InsertOneAsync(new TestEntity(id)
                {
                    Quantity = quantity
                });
            return id;
        }
    }
}
