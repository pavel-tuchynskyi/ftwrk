using FTWRK.Application.Common.DTO.Images;
using MediatR;

namespace FTWRK.Application.Images.Queries.GetPictureByName
{
    public class GetPictureByNameQuery : IRequest<ImageDto>
    {
        public string Name { get; set; }
    }
}
