using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Playlists;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FTWRK.Application.Playlists.Commands.CreatePlaylist
{
    public class CreatePlaylistCommand : IRequest<Guid>, IMap<CustomPlaylist>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? Poster { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePlaylistCommand, CustomPlaylist>()
                .ForMember(x => x.Songs, x => x.MapFrom(x => new List<PlaylistSong>()))
                .ForMember(x => x.Poster, x => x.Ignore());
        }
    }
}
