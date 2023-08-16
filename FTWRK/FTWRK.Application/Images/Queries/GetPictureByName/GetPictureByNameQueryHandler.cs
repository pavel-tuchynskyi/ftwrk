using AutoMapper;
using FTWRK.Application.Common.DTO.Images;
using FTWRK.Application.Common.Interfaces;
using MediatR;

namespace FTWRK.Application.Images.Queries.GetPictureByName
{
    public class GetPictureByNameQueryHandler : IRequestHandler<GetPictureByNameQuery, ImageDto>
    {
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public GetPictureByNameQueryHandler(IImageService imageService, IMapper mapper)
        {
            _imageService = imageService;
            _mapper = mapper;
        }
        public async Task<ImageDto> Handle(GetPictureByNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _imageService.GetImageByName(request.Name);

            var resultDto = _mapper.Map<ImageDto>(result);

            return resultDto;
        }
    }
}
