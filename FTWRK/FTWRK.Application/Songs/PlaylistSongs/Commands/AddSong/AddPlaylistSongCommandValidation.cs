using FluentValidation;

namespace FTWRK.Application.Songs.PlaylistSongs.Commands.AddSong
{
    public class AddPlaylistSongCommandValidation : AbstractValidator<AddPlaylistSongCommand>
    {
        public AddPlaylistSongCommandValidation()
        {
            RuleFor(x => x.AlbumId).NotEmpty().WithMessage("AlbumId is reqired");
            RuleFor(x => x.PlaylistId).NotEmpty().WithMessage("PlaylistId is reqired");
            RuleFor(x => x.SongId).NotEmpty().WithMessage("SongId is reqired");
        }
    }
}
