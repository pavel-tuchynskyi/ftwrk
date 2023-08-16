using FTWRK.Application.Common.DTO.Playlists;
using FTWRK.Application.Playlists.Queries.GetFavoritePlaylist;
using MediatR;

namespace FTWRK.Application.Playlists.Queries.GetCustomPlaylist
{
    public class GetCustomPlaylistQuery : IRequest<CustomPlaylistDetailsDto>
    {
        public Guid Id { get; set; }
    }
}
