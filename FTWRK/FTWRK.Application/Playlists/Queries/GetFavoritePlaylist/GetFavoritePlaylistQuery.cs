using FTWRK.Application.Common.DTO.Playlists;
using MediatR;

namespace FTWRK.Application.Playlists.Queries.GetFavoritePlaylist
{
    public class GetFavoritePlaylistQuery : IRequest<PlaylistDetailsDto>
    {
    }
}
