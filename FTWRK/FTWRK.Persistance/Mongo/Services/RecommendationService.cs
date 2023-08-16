using FTWRK.Application.Common.DTO.Analytics;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Domain.Entities.Analythics;
using FTWRK.Persistance.Common.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FTWRK.Persistance.Mongo.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IMongoCollection<ListeningHistory> _collection;
        private readonly IMongoContext _dbContext;

        public RecommendationService(IMongoContext dbContext)
        {
            _collection = dbContext.GetCollection<ListeningHistory>();
            _dbContext = dbContext;
        }

        public async Task<List<Album>> GetRecommendations(Guid userId, List<Guid> userAlbums, List<string> genres)
        {
            var neighbours = await GetNeighbours(userAlbums, userId);
            var recommendations = await GetRecommendedAlbums(neighbours, genres);

            return recommendations;
        }

        public async Task<List<Album>> GetRecommendedAlbums(List<Guid> neighbours, List<string> genres)
        {
            var albumsCollentionName = _dbContext.GetCollectionName<Album>();

            var albumRootPipe = new BsonDocument("$replaceRoot", new BsonDocument
            {
                { "newRoot", $"${nameof(ListeningHistoryDTO.Albums)}" }
            });

            var recommendations = await _collection.Aggregate()
                .Match(Builders<ListeningHistory>.Filter.In(x => x.UserId, neighbours))
                .Lookup(albumsCollentionName, $"{nameof(ListeningHistory.AlbumId)}", "_id", $"{nameof(ListeningHistoryDTO.Albums)}")
                .Unwind($"{nameof(ListeningHistoryDTO.Albums)}")
                .Match(Builders<BsonDocument>.Filter.AnyIn($"{nameof(ListeningHistoryDTO.Albums)}.{nameof(Album.Genres)}", genres))
                .AppendStage<BsonDocument>(albumRootPipe)
                .As<Album>()
                .Limit(6)
                .ToListAsync();

            return recommendations;
        }

        public async Task<List<Guid>> GetNeighbours(List<Guid> userAlbums, Guid userId)
        {
            var neighbours = await _collection
                .DistinctAsync(x => x.UserId, Builders<ListeningHistory>.Filter.And(
                    Builders<ListeningHistory>.Filter.In(x => x.AlbumId, userAlbums),
                    Builders<ListeningHistory>.Filter.Not(Builders<ListeningHistory>.Filter.Eq(x => x.UserId, userId))))
                .Result
                .ToListAsync();

            return neighbours;
        }
    }
}
