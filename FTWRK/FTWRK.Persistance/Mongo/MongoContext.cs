using FTWRK.Domain.Attributes;
using FTWRK.Infrastructure;
using FTWRK.Persistance.Common.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;
using Serilog;
using System.Reflection;

namespace FTWRK.Persistance.Mongo
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _mongoClient;

        public MongoContext(IMongoClient mongoClient, IOptions<MongoOptions> options)
        {
            var settings = options.Value;
            _mongoClient = mongoClient;
            _database = _mongoClient.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<TData> GetCollection<TData>()
        {
            var name = GetCollectionName<TData>();
            var collection = _database.GetCollection<TData>(name);

            if(collection == null)
            {
                Log.Error("Can't find collection: {name}", name);
                throw new ArgumentException("Can't find this collection");
            }

            return collection;
        }

        public string GetCollectionName<TData>()
        {
            string collectionName = null;

            if(typeof(TData).GetCustomAttribute<BsonCollectionAttribute>() != null)
            {
                collectionName = typeof(TData).GetCustomAttribute<BsonCollectionAttribute>().CollectionName;
            }
            else if(typeof(TData).GetCustomAttribute<CollectionNameAttribute>() != null)
            {
                collectionName = typeof(TData).GetCustomAttribute<CollectionNameAttribute>().Name;
            }
            else
            {
                Log.Error("Unknown collection name for: {name}", typeof(TData).Name);
                throw new ArgumentException("Unknown collection name");
            }

            return collectionName;
        }

        public async Task<IClientSessionHandle> StartSessionAsync()
        {
            return await _mongoClient.StartSessionAsync(new ClientSessionOptions());
        }
    }
}
