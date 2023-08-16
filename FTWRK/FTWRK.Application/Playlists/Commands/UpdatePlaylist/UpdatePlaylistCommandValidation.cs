using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Playlists.Commands.UpdatePlaylist
{
    public class UpdatePlaylistCommandValidation : CustomAbstractValidator<UpdatePlaylistCommand>
    {
        public UpdatePlaylistCommandValidation()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is requered");
            RuleFor(x => x.OwnerId).NotEmpty().WithMessage("OwnerId is reqired");
        }
    }
}
