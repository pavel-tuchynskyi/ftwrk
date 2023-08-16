using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Images;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FTWRK.Application.Images.Commands.UploadPicture
{
    public class UploadPictureCommand : IRequest<Unit>, IMap<Image>
    {
        public string Name { get; set; }
        public IFormFile Picture { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UploadPictureCommand, Image>()
                .ForMember(x => x.ImageBlob, x => x.Ignore())
                .ReverseMap();
        }
    }
}
