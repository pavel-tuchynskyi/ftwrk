using FluentValidation;

namespace FTWRK.Application.Songs.PlaylistSongs.Commands.DeleteSong
{
    public class DeletePlaylistSongCommandValidation : AbstractValidator<DeletePlaylistSongCommand>
    {
        public DeletePlaylistSongCommandValidation()
        {
            RuleFor(x => x.PlaylistId).NotEmpty().WithMessage("PlaylistId is reqired");
            RuleFor(x => x.SongId).NotEmpty().WithMessage("SongId is reqired");
        }
    }
}
