using FluentValidation;

namespace FTWRK.Application.Songs.AlbumSongs.Commands.AddSong
{
    public class AddAlbumSongCommandValidation : AbstractValidator<AddAlbumSongCommand>
    {
        public AddAlbumSongCommandValidation()
        {
            RuleFor(x => x.AlbumId).NotEmpty().WithMessage("AlbumId is reqired");
            RuleFor(x => x.Artists).NotEmpty().WithMessage("Artists can't be null");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is reqired");
            RuleFor(x => x.SongBlob).NotEmpty().WithMessage("Song file can't be null");
        }
    }
}
