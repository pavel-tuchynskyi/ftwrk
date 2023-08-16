using FTWRK.Application.Common.DTO.Albums;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Albums.Queries.GetAlbums
{
    public class GetAlbumsQuery : QueryParameters, IRequest<PagedList<AlbumDto>>
    {
    }
}
