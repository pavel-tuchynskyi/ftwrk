using AutoMapper;
using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Helpers;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Images;
using FTWRK.Domain.Entities.Playlists;
using MediatR;

namespace FTWRK.Application.Playlists.Commands.UpdatePlaylist
{
    public class UpdatePlaylistCommandHandler : IRequestHandler<UpdatePlaylistCommand, Unit>
    {
        private readonly IPlaylistService _playlistService;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public UpdatePlaylistCommandHandler(IPlaylistService playlistService, IMapper mapper, IUserContextService userContext)
        {
            _playlistService = playlistService;
            _mapper = mapper;
            _userId = userContext.GetUserId();
        }

        public async Task<Unit> Handle(UpdatePlaylistCommand request, CancellationToken cancellationToken)
        {
            if(_userId != request.OwnerId)
            {
                throw new BadRequestException("User isn't creator of this album");
            }

            var playlist = _mapper.Map<CustomPlaylist>(request);

            if (request.Poster != null)
            {
                var imageBytes = await FileHelper.SerializeImageAsync(request.Poster);
                playlist.Poster = new ImageBlob(request.Poster.ContentType, imageBytes);
            }

            await _playlistService.Update(request.Id, playlist);

            return Unit.Value;
        }
    }
}
