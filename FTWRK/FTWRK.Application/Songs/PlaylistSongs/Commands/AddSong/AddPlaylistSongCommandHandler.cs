using AutoMapper;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Analythics;
using FTWRK.Domain.Entities.Playlists;
using MediatR;

namespace FTWRK.Application.Songs.PlaylistSongs.Commands.AddSong
{
    public class AddPlaylistSongCommandHandler : IRequestHandler<AddPlaylistSongCommand, Unit>
    {
        private readonly ISongServiceFactory _serviceFactory;
        private readonly IMapper _mapper;
        private readonly IUserAnalyticsService _analytics;
        private readonly Guid _userId;

        public AddPlaylistSongCommandHandler(ISongServiceFactory serviceFactory, IUserContextService userContext,
            IMapper mapper, IUserAnalyticsService analytics)
        {
            _serviceFactory = serviceFactory;
            _mapper = mapper;
            _analytics = analytics;
            _userId = userContext.GetUserId();
        }

        public async Task<Unit> Handle(AddPlaylistSongCommand request, CancellationToken cancellationToken)
        {
            var playlistSong = _mapper.Map<PlaylistSong>(request);
            var songService = _serviceFactory.GetSongService(SongType.Playlist);
            await songService.Add(request.PlaylistId, _userId, playlistSong);

            await _analytics.Add(new ListeningHistory
            {
                UserId = _userId,
                AlbumId = request.AlbumId
            });

            return Unit.Value;
        }
    }
}
