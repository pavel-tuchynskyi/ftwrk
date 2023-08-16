using FTWRK.Application.Common.Models;
using FTWRK.Persistance.Common.Interfaces;

namespace FTWRK.Persistance.Mongo.Services.Strategies.GetPlaylistSongsStrategy
{
    public class GetPlaylistSongsResolver
    {
        private readonly IMongoContext _dbContext;

        public GetPlaylistSongsResolver(IMongoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IPlaylistSongs GetStrategy(QueryParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.OrderBy))
            {
                return new GetPlaylistSongsQueryParams(_dbContext);
            }
            else
            {
                return new GetPlaylistSongsLinkedList(_dbContext);
            }
        }
    }
}
