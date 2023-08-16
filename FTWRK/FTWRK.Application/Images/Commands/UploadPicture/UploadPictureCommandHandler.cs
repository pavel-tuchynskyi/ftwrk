using AutoMapper;
using FTWRK.Application.Common.Helpers;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Images;
using MediatR;

namespace FTWRK.Application.Images.Commands.UploadPicture
{
    public class UploadPictureCommandHandler : IRequestHandler<UploadPictureCommand, Unit>
    {
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public UploadPictureCommandHandler(IImageService imageService, IMapper mapper)
        {
            _imageService = imageService;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UploadPictureCommand request, CancellationToken cancellationToken)
        {
            var image = _mapper.Map<Image>(request);
            var imageBytes = await FileHelper.SerializeImageAsync(request.Picture);
            image.ImageBlob = new ImageBlob(request.Picture.ContentType, imageBytes);

            await _imageService.UploadPicture(image);

            return Unit.Value;
        }
    }
}
