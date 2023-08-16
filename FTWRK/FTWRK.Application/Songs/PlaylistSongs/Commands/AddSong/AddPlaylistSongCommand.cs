using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Playlists;
using MediatR;

namespace FTWRK.Application.Songs.PlaylistSongs.Commands.AddSong
{
    public class AddPlaylistSongCommand : IRequest<Unit>, IMap<PlaylistSong>
    {
        public Guid PlaylistId { get; set; }
        public Guid SongId { get; set; }
        public Guid AlbumId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddPlaylistSongCommand, PlaylistSong>();
        }
    }
}
