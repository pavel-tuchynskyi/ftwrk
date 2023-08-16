using FTWRK.Application.Common.DTO.Playlists;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Playlists;
using FTWRK.Infrastructure.Idenity.Models;
using FTWRK.Persistance.Common.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FTWRK.Persistance.Mongo.Services.Strategies.GetPlaylistStrategy
{
    public class GetCustomPlaylistStrategy : IPlaylist
    {
        public readonly PlaylistType playlistType = PlaylistType.Custom;
        private readonly IMongoCollection<CustomPlaylist> _collection;

        public GetCustomPlaylistStrategy(IMongoCollection<CustomPlaylist> collection)
        {
            _collection = collection;
        }
        public async Task<PlaylistDetailsDto> GetPlaylist(Guid id)
        {
            var playlist = await _collection.Aggregate()
                .Match(x => x.Id == id)
                .Lookup("Users", nameof(CustomPlaylist.OwnerId), "_id", nameof(ApplicationUser))
                .Unwind(nameof(ApplicationUser))
                .Project(new BsonDocument
                {
                    { nameof(CustomPlaylistDetailsDto.OwnerName), $"${nameof(ApplicationUser)}.{nameof(ApplicationUser.FullName)}"},
                    { nameof(CustomPlaylistDetailsDto.OwnerId), $"${nameof(ApplicationUser)}._id"},
                    { nameof(CustomPlaylistDetailsDto.Title), $"${nameof(CustomPlaylist.Title)}" },
                    { nameof(CustomPlaylistDetailsDto.Description), $"${nameof(CustomPlaylist.Description)}" },
                    { nameof(CustomPlaylistDetailsDto.Poster), $"${nameof(CustomPlaylist.Poster)}" }
                })
                .As<CustomPlaylistDetailsDto>()
                .FirstOrDefaultAsync();

            return playlist;
        }
    }
}
