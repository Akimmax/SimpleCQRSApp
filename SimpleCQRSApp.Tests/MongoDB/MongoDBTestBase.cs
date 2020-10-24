using MongoDB.Driver;
using System;

namespace SimpleCQRSApp.Tests
{
    public class MongoDBTestBase : IDisposable
    {
        protected IMongoDatabase mongoDB;
        protected MongoClient client;

        protected string dbName = "cqrs1";

        public MongoDBTestBase(string database = null)
        {           
            client = new MongoClient("mongodb://localhost:27017");
            mongoDB = client.GetDatabase(dbName);
        }

        public void Dispose()
        {

            //TODO Remove comments 
            //client.DropDatabase(dbName);
        }
    }
}
