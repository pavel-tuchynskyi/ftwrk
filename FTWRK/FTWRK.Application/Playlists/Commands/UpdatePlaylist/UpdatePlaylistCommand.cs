using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Playlists;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FTWRK.Application.Playlists.Commands.UpdatePlaylist
{
    public class UpdatePlaylistCommand : IRequest<Unit>, IMap<CustomPlaylist>
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? Poster { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePlaylistCommand, CustomPlaylist>()
                .ForMember(x => x.Poster, x => x.Ignore())
                .ForMember(x => x.Description, x => x.AllowNull());
        }
    }
}
