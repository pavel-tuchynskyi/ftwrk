using FTWRK.Application.Common.DTO.Analytics;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Domain.Entities.Analythics;
using FTWRK.Persistance.Common.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FTWRK.Persistance.Mongo.Services
{
    public class UserAnalyticsService : IUserAnalyticsService
    {
        private readonly IMongoCollection<ListeningHistory> _collection;
        private readonly IMongoContext _dbContext;

        public UserAnalyticsService(IMongoContext dbContext)
        {
            _collection = dbContext.GetCollection<ListeningHistory>();
            _dbContext = dbContext;
        }

        public async Task<List<ListeningHistoryDTO>> GetById(Guid userId)
        {
            var albumsCollentionName = _dbContext.GetCollectionName<Album>();
            var sortPipeline = new SortDefinitionBuilder<ListeningHistoryDTO>().Descending(x => x.ListeningDate).Descending(x => x.Genre);
            
            var result = await _collection.Aggregate()
            .Match(x => x.UserId == userId)
            .Lookup(albumsCollentionName, $"{nameof(ListeningHistory.AlbumId)}", "_id", $"{nameof(Album)}")
            .Unwind($"{nameof(Album)}")
            .Unwind($"{nameof(Album)}.{nameof(Album.Genres)}")
            .Group(new BsonDocument{
                { "_id", $"${nameof(Album)}.{nameof(Album.Genres)}" },
                { $"{nameof(ListeningHistoryDTO.Albums)}", new BsonDocument{
                    { "$addToSet", $"${nameof(ListeningHistory.AlbumId)}" }
                } },
                { $"{nameof(ListeningHistoryDTO.UserId)}", new BsonDocument{
                    { "$first", $"${nameof(ListeningHistory.UserId)}" }
                } },
                { $"{nameof(ListeningHistoryDTO.Count)}", new BsonDocument("$sum", 1) },
                { $"{nameof(ListeningHistoryDTO.ListeningDate)}", new BsonDocument{
                    { "$first", $"${nameof(ListeningHistory.ListeningDate)}" }
                } }
            })
            .Project(new BsonDocument
            {
                { "_id", 0 },
                { $"{nameof(ListeningHistoryDTO.Genre)}", "$_id" },
                { $"{nameof(ListeningHistoryDTO.Count)}", 1 },
                { $"{nameof(ListeningHistoryDTO.Albums)}", 1 },
                { $"{nameof(ListeningHistoryDTO.UserId)}", 1 },
                { $"{nameof(ListeningHistoryDTO.ListeningDate)}", 1 },
            })
            .As<ListeningHistoryDTO>()
            .Sort(sortPipeline)
            .Limit(5)
            .ToListAsync();

            return result;
        }

        public async Task<bool> Add(ListeningHistory history)
        {
            await _collection.InsertOneAsync(history);

            return true;
        }
    }
}
