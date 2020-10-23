using SimpleCQRSApp.Infrastructure.Read;

namespace SimpleCQRSApp.Tests
{
    public class TestEntity : IReadEntity
    {
        private TestEntity()
        {

        }

        public TestEntity(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }

        public int Quantity { get; set; }
    }
}
