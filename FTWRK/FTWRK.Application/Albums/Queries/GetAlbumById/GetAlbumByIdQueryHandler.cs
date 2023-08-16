using FTWRK.Application.Common.DTO.Albums;
using FTWRK.Application.Common.Interfaces;
using MediatR;

namespace FTWRK.Application.Albums.Queries.GetAlbumById
{
    public class GetAlbumByIdQueryHandler : IRequestHandler<GetAlbumByIdQuery, AlbumDetailsDto>
    {
        private readonly IAlbumService _albumService;

        public GetAlbumByIdQueryHandler(IAlbumService albumService)
        {
            _albumService = albumService;
        }
        public async Task<AlbumDetailsDto> Handle(GetAlbumByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _albumService.GetById(request.Id);

            return result;
        }
    }
}
