using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Domain.Entities.Songs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FTWRK.Application.Albums.Commands.UpdateAlbum
{
    public class UpdateAlbumCommand : IRequest<Unit>, IMap<Album>
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public string? Title { get; set; }
        public int Year { get; set; }
        public List<string>? Genres { get; set; }
        public AlbumType AlbumType { get; set; }
        public IFormFile? Poster { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAlbumCommand, Album>()
                .ForMember(x => x.Genres, x => x.AllowNull())
                .ForMember(x => x.Title, x => x.AllowNull())
                .ForMember(x => x.Poster, x => x.Ignore())
                .ForMember(x => x.Songs, x => x.Ignore())
                .ReverseMap();
        }
    }
}
