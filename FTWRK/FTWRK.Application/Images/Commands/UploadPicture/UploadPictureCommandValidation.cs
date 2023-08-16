using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Images.Commands.UploadPicture
{
    public class UploadPictureCommandValidation : CustomAbstractValidator<UploadPictureCommand>
    {
        public UploadPictureCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is requered");
            RuleFor(x => x.Picture).NotNull().WithMessage("Picture is required");
        }
    }
}
