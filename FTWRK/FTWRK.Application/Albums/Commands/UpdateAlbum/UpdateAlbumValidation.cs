using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Albums.Commands.UpdateAlbum
{
    public class UpdateAlbumValidation : CustomAbstractValidator<UpdateAlbumCommand>
    {
        public UpdateAlbumValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Invalid Title");
            RuleFor(x => x.Id).NotEmpty().WithMessage("Invalid album id");
        }
    }
}
