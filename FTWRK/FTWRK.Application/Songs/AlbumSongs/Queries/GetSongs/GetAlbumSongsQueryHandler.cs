using AutoMapper;
using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Songs.AlbumSongs.Queries.GetSongs
{
    public class GetAlbumSongsQueryHandler : IRequestHandler<GetAlbumSongsQuery, PagedList<SongDto>>
    {
        private readonly ISongServiceFactory _serviceFactory;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public GetAlbumSongsQueryHandler(ISongServiceFactory serviceFactory, IMapper mapper, IUserContextService userContext)
        {
            _serviceFactory = serviceFactory;
            _mapper = mapper;
            _userId = userContext.GetUserId();
        }
        public async Task<PagedList<SongDto>> Handle(GetAlbumSongsQuery request, CancellationToken cancellationToken)
        {
            var requestParams = _mapper.Map<QueryParameters>(request);
            var albumSongService = _serviceFactory.GetSongService(SongType.Album);

            var songs = await albumSongService.GetAll(requestParams, _userId, request.AlbumId);

            return songs;
        }
    }
}
