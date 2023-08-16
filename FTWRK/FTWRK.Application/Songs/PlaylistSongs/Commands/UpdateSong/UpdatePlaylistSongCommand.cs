using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Playlists;
using MediatR;

namespace FTWRK.Application.Songs.PlaylistSongs.Commands.UpdateSong
{
    public class UpdatePlaylistSongCommand: IRequest<Unit>, IMap<PlaylistSong>
    {
        public Guid PlaylistId { get; set; }
        public Guid SongId { get; set; }
        public Guid AlbumId { get; set; }
        public Guid? Previous { get; set; }
        public Guid? Next { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePlaylistSongCommand, PlaylistSong>();
        }
    }
}
