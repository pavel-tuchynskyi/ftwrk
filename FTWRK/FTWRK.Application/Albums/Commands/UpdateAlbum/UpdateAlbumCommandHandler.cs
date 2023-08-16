using AutoMapper;
using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Helpers;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Domain.Entities.Images;
using MediatR;
using Serilog;

namespace FTWRK.Application.Albums.Commands.UpdateAlbum
{
    public class UpdateAlbumCommandHandler : IRequestHandler<UpdateAlbumCommand, Unit>
    {
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public UpdateAlbumCommandHandler(IAlbumService albumService, IMapper mapper, IUserContextService userContext)
        {
            _albumService = albumService;
            _mapper = mapper;
            _userId = userContext.GetUserId();
        }
        public async Task<Unit> Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
        {
            if(_userId != request.CreatorId)
            {
                Log.Error("User: {userId} is not creator of album: {albumId}", _userId, request.Id);
                throw new BadRequestException("User isn't creator of this album");
            }

            var album = _mapper.Map<Album>(request);

            if(request.Poster != null)
            {
                var imageBytes = await FileHelper.SerializeImageAsync(request.Poster);
                album.Poster = new ImageBlob(request.Poster.ContentType, imageBytes);
            }

            await _albumService.Update(request.Id, album);

            return Unit.Value;
        }
    }
}
