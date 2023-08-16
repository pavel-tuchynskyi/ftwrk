using FluentValidation;

namespace FTWRK.Application.Playlists.Commands.DeletePlaylist
{
    public class DeletePlaylistCommandValidation : AbstractValidator<DeletePlaylistCommand>
    {
        public DeletePlaylistCommandValidation()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Playlist id is reqired");
        }
    }
}
