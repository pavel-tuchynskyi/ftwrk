using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Images.Queries.GetPictureByName
{
    public class GetPictureByNameValidation : CustomAbstractValidator<GetPictureByNameQuery>
    {
        public GetPictureByNameValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Image name is required");
        }
    }
}
