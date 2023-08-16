using AutoMapper;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Playlists;
using MediatR;

namespace FTWRK.Application.Songs.PlaylistSongs.Commands.UpdateSong
{
    public class UpdatePlaylistSongCommandHandler : IRequestHandler<UpdatePlaylistSongCommand, Unit>
    {
        private readonly ISongServiceFactory _serviceFactory;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public UpdatePlaylistSongCommandHandler(ISongServiceFactory serviceFactory, IMapper mapper, IUserContextService userContext)
        {
            _serviceFactory = serviceFactory;
            _mapper = mapper;
            _userId = userContext.GetUserId();
        }
        public async Task<Unit> Handle(UpdatePlaylistSongCommand request, CancellationToken cancellationToken)
        {
            var playlistSong = _mapper.Map<PlaylistSong>(request);
            var songService = _serviceFactory.GetSongService(SongType.Playlist);

            await songService.Update(request.PlaylistId, _userId, playlistSong);

            return Unit.Value;
        }
    }
}
