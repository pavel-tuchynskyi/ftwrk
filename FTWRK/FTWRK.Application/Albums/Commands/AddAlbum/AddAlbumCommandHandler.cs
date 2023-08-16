using AutoMapper;
using FTWRK.Application.Common.Constants;
using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Helpers;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Domain.Entities.Images;
using MediatR;

namespace FTWRK.Application.Albums.Commands.AddAlbum
{
    public class AddAlbumCommandHandler : IRequestHandler<AddAlbumCommand, Guid>
    {
        private readonly IAlbumService _albumService;
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;
        private readonly Guid _userId;

        public AddAlbumCommandHandler(IAlbumService albumService, IMapper mapper, IUserManager userManager,
            IUserContextService userContext)
        {
            _albumService = albumService;
            _mapper = mapper;
            _userManager = userManager;
            _userId = userContext.GetUserId();
        }

        public async Task<Guid> Handle(AddAlbumCommand request, CancellationToken cancellationToken)
        {
            if (!await _userManager.IsUserInRole(_userId, RoleNames.Artist))
            {
                throw new BadRequestException("User isn't artist");
            }

            var album = _mapper.Map<Album>(request);
            album.CreatorId = _userId;

            var imageBytes = await FileHelper.SerializeImageAsync(request.Poster);
            album.Poster = new ImageBlob(request.Poster.ContentType, imageBytes);

            await _albumService.Add(album);
            return album.Id;
        }
    }
}
