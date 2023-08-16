using FTWRK.Application.Common.DTO.Playlists;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Playlists.Queries.GetPlaylists
{
    public class GetPlaylistsQuery : QueryParameters, IRequest<PagedList<CustomPlaylistDetailsDto>>
    {
    }
}
