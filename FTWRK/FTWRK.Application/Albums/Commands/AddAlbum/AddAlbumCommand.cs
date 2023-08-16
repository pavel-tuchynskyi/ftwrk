using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Domain.Entities.Songs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FTWRK.Application.Albums.Commands.AddAlbum
{
    public class AddAlbumCommand : IRequest<Guid>, IMap<Album>
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public List<string> Genres { get; set; }
        public AlbumType AlbumType { get; set; }
        public IFormFile Poster { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddAlbumCommand, Album>()
                .ForMember(x => x.Poster, x => x.Ignore())
                .ForMember(x => x.Songs, x => x.MapFrom(x => new List<Song>()))
                .ReverseMap();
        }
    }
}
