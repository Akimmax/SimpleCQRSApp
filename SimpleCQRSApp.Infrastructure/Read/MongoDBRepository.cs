using SimpleCQRSApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
namespace SimpleCQRSApp.Infrastructure.Read
{
    public class MongoDBRepository<T> : IRepository<T>
        where T : IReadEntity
    {
        private readonly IMongoDatabase mongoDatabase;

        public MongoDBRepository(IMongoDatabase mongoDatabase)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var mongoDB = client.GetDatabase("cqrs1");
            // TODO fix issue with DI
            this.mongoDatabase = mongoDB;//mongoDatabase;
        }

        private string CollectionName => typeof(T).Name;

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            var cursor = await mongoDatabase.GetCollection<T>(CollectionName)
                .FindAsync(predicate);
            return cursor.ToEnumerable();
        }

        public Task<T> GetByIdAsync(string id)
        {
            return mongoDatabase.GetCollection<T>(CollectionName)
                .Find(x => x.Id == id).SingleOrDefaultAsync();
            //.Find(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task InsertAsync(T entity)
        {
            try
            {
                await mongoDatabase.GetCollection<T>(CollectionName)
                    .InsertOneAsync(entity);
            }
            catch (MongoWriteException ex)
            {
                throw new RepositoryException($"Error inserting entity {entity.Id}", ex);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                var result = await mongoDatabase.GetCollection<T>(CollectionName)
                    .ReplaceOneAsync(x => x.Id == entity.Id, entity);

                if (result.MatchedCount != 1)
                {
                    throw new RepositoryException($"Missing entoty {entity.Id}");
                }
            }
            catch (MongoWriteException ex)
            {
                throw new RepositoryException($"Error updating entity {entity.Id}", ex);
            }
        }
    }
}
