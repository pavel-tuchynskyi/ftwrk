using FluentValidation;

namespace FTWRK.Application.Albums.Commands.DeleteAlbum
{
    public class DeleteAlbumCommandValidation : AbstractValidator<DeleteAlbumCommand>
    {
        public DeleteAlbumCommandValidation()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Album id is reqired");
        }
    }
}
