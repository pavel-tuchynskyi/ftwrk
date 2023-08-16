using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Songs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FTWRK.Application.Songs.AlbumSongs.Commands.AddSong
{
    public class AddAlbumSongCommand : IRequest<Unit>, IMap<AlbumSong>
    {
        public Guid AlbumId { get; set; }
        public List<SongArtist> Artists { get; set; }
        public string Title { get; set; }
        public IFormFile SongBlob { get; set; }
        public string ConnectionId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddAlbumSongCommand, AlbumSong>()
                .ForMember(x => x.Song, x => x.MapFrom(y => new Song
                {
                    Artists = y.Artists,
                    Title = y.Title
                }))
                .ForMember(x => x.SongBlob, x => x.Ignore());
        }
    }
}
