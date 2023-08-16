using FTWRK.Application.Common.DTO.Albums;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Albums.Queries.GetAlbums
{
    public class GetAlbumsQueryHandler : IRequestHandler<GetAlbumsQuery, PagedList<AlbumDto>>
    {
        private readonly IAlbumService _albumService;

        public GetAlbumsQueryHandler(IAlbumService albumService)
        {
            _albumService = albumService;
        }
        public async Task<PagedList<AlbumDto>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
        {
            var result = await _albumService.GetAll(request);

            return result;
        }
    }
}
