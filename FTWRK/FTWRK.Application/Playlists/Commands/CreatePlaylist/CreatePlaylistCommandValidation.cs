using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Playlists.Commands.CreatePlaylist
{
    public class CreatePlaylistCommandValidation : CustomAbstractValidator<CreatePlaylistCommand>
    {
        public CreatePlaylistCommandValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is requered");
        }
    }
}
