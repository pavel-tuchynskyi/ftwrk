using FTWRK.Application.Common.DTO.Albums;
using MediatR;

namespace FTWRK.Application.Albums.Queries.GetAlbumById
{
    public class GetAlbumByIdQuery : IRequest<AlbumDetailsDto>
    {
        public Guid Id { get; set; }
    }
}
