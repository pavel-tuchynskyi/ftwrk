using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Albums.Commands.AddAlbum
{
    public class AddAlbumCommandValidation : CustomAbstractValidator<AddAlbumCommand>
    {
        public AddAlbumCommandValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Invalid Title");
            RuleFor(x => x.Year).NotEmpty().GreaterThan(1900).WithMessage("Year must be greater then 1900");
        }
    }
}
