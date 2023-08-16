using AutoMapper;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Songs;
using MediatR;

namespace FTWRK.Application.Songs.AlbumSongs.Commands.UpdateSong
{
    public class UpdateAlbumSongCommandHandler : IRequestHandler<UpdateAlbumSongCommand, Unit>
    {
        private readonly ISongServiceFactory _serviceFactory;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public UpdateAlbumSongCommandHandler(ISongServiceFactory serviceFactory, IMapper mapper, IUserContextService userContext)
        {
            _serviceFactory = serviceFactory;
            _mapper = mapper;
            _userId = userContext.GetUserId();
        }
        public async Task<Unit> Handle(UpdateAlbumSongCommand request, CancellationToken cancellationToken)
        {
            var song = _mapper.Map<Song>(request);

            var songService = _serviceFactory.GetSongService(SongType.Album);
            await songService.Update(request.AlbumId, _userId, song);

            return Unit.Value;
        }
    }
}
