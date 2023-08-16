using AutoMapper;
using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Songs.PlaylistSongs.Queries.GetPlaylistSongs
{
    public class GetPlaylistSongsQueryHandler : IRequestHandler<GetPlaylistSongsQuery, PagedList<SongDto>>
    {
        private readonly ISongServiceFactory _serviceFactory;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public GetPlaylistSongsQueryHandler(ISongServiceFactory serviceFactory, IMapper mapper, IUserContextService userContext)
        {
            _serviceFactory = serviceFactory;
            _mapper = mapper;
            _userId = userContext.GetUserId();
        }
        public async Task<PagedList<SongDto>> Handle(GetPlaylistSongsQuery request, CancellationToken cancellationToken)
        {
            var requestParams = _mapper.Map<QueryParameters>(request);
            var playlistSongService = _serviceFactory.GetSongService(SongType.Playlist);
            var playlistSongs = await playlistSongService.GetAll(requestParams, _userId, request.PlaylistId);

            return playlistSongs;
        }
    }
}
