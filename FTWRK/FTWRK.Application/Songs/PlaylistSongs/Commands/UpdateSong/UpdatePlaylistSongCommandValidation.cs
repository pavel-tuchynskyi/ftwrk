using FluentValidation;

namespace FTWRK.Application.Songs.PlaylistSongs.Commands.UpdateSong
{
    public class UpdatePlaylistSongCommandValidation : AbstractValidator<UpdatePlaylistSongCommand>
    {
        public UpdatePlaylistSongCommandValidation()
        {
            RuleFor(x => x.PlaylistId).NotEmpty().WithMessage("PlaylistId is reqired");
            RuleFor(x => x.SongId).NotEmpty().WithMessage("SongId is reqired");
            RuleFor(x => x.AlbumId).NotEmpty().WithMessage("AlbumId is reqired");
        }
    }
}
