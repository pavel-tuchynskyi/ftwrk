using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Images;

namespace FTWRK.Application.Common.DTO.Images
{
    public class ImageDto : IMap<Image>
    {
        public string ImageType { get; set; }
        public byte[] ImageBytes { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Image, ImageDto>()
                .ForMember(x => x.ImageType, x => x.MapFrom(y => y.ImageBlob.ImageType))
                .ForMember(x => x.ImageBytes, x => x.MapFrom(y => y.ImageBlob.ImageBytes));
        }
    }
}
