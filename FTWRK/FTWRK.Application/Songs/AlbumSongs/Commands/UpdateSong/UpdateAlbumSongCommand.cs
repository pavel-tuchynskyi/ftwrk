using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Songs;
using MediatR;

namespace FTWRK.Application.Songs.AlbumSongs.Commands.UpdateSong
{
    public class UpdateAlbumSongCommand : IRequest<Unit>, IMap<Song>
    {
        public Guid AlbumId { get; set; }
        public Guid SongId { get; set; }
        public string Title { get; set; }
        public List<SongArtist> Artists { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAlbumSongCommand, Song>()
                .ForMember(x => x.Id, x => x.MapFrom(y => y.SongId));
        }
    }
}
