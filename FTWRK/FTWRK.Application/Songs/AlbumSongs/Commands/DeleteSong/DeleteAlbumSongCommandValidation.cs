using FluentValidation;

namespace FTWRK.Application.Songs.AlbumSongs.Commands.DeleteSong
{
    public class DeleteAlbumSongCommandValidation : AbstractValidator<DeleteAlbumSongCommand>
    {
        public DeleteAlbumSongCommandValidation()
        {
            RuleFor(x => x.SongId).NotEmpty().WithMessage("SongId is reqired");
            RuleFor(x => x.AlbumId).NotEmpty().WithMessage("AlbumId is reqired");
        }
    }
}
