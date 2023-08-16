using AutoMapper;
using FTWRK.Application.Common.Helpers;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Images;
using FTWRK.Domain.Entities.Playlists;
using MediatR;

namespace FTWRK.Application.Playlists.Commands.CreatePlaylist
{
    public class CreatePlaylistCommandHandler : IRequestHandler<CreatePlaylistCommand, Guid>
    {
        private readonly IPlaylistService _playlistService;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public CreatePlaylistCommandHandler(IPlaylistService playlistService, IMapper mapper, IUserContextService userContext)
        {
            _playlistService = playlistService;
            _mapper = mapper;
            _userId = userContext.GetUserId();
        }
        public async Task<Guid> Handle(CreatePlaylistCommand request, CancellationToken cancellationToken)
        {
            var playlist = _mapper.Map<CustomPlaylist>(request);
            playlist.OwnerId = _userId;

            if (request.Poster != null)
            {
                var imageBytes = await FileHelper.SerializeImageAsync(request.Poster);
                playlist.Poster = new ImageBlob(request.Poster.ContentType, imageBytes);
            }

            await _playlistService.Add(playlist);

            return playlist.Id;
        }
    }
}
