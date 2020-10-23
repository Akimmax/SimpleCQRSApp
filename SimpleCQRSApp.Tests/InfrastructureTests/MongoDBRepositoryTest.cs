using SimpleCQRSApp.Infrastructure.Persistence;
using SimpleCQRSApp.Infrastructure.Read;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SimpleCQRSApp.Tests;

namespace SimpleCQRSApp.Tests
{
    [Trait("Type", "Integration")]
    public class MongoDBRepositoryTest : MongoDBTestBase
    {
        //TODO remove harcode
        private const string Id = "2";

        private readonly MongoDBRepository<TestEntity> sut;

        public MongoDBRepositoryTest()
        {
            sut = new MongoDBRepository<TestEntity>(mongoDB);
        }

        [Fact]
        public async Task CanInsert()
        {
            await sut.InsertAsync(new TestEntity(Id) { Quantity = 3 });

            var test = await sut.GetByIdAsync(Id);

            Assert.NotNull(test);
            Assert.Equal(Id, test.Id);
        }

        [Fact]
        public async Task CanUpdate()
        {
            await sut.InsertAsync(new TestEntity(Id) { Quantity = 3 });
            await sut.UpdateAsync(new TestEntity(Id) { Quantity = 10 });

            var test = await sut.GetByIdAsync(Id);

            Assert.NotNull(test);
            Assert.Equal(10, test.Quantity);
        }

        [Fact]
        public async Task CanFindWithCondition()
        {
            await sut.InsertAsync(new TestEntity("1") { Quantity = 10 });
            await sut.InsertAsync(new TestEntity("2") { Quantity = 11 });
            await sut.InsertAsync(new TestEntity("3") { Quantity = 12 });
            await sut.InsertAsync(new TestEntity("4") { Quantity = 13 });

            var list = (await sut.FindAllAsync(x => x.Quantity > 10 && x.Quantity < 13)).ToList();

            Assert.Collection(list, x => Assert.Equal("2", x.Id), x => Assert.Equal("3", x.Id));
        }
    }
}
