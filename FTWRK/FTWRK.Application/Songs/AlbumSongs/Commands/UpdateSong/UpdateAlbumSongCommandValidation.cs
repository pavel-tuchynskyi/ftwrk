using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTWRK.Application.Songs.AlbumSongs.Commands.UpdateSong
{
    public class UpdateAlbumSongCommandValidation : AbstractValidator<UpdateAlbumSongCommand>
    {
        public UpdateAlbumSongCommandValidation()
        {
            RuleFor(x => x.AlbumId).NotEmpty().WithMessage("AlbumId is reqired");
            RuleFor(x => x.Artists).NotEmpty().WithMessage("Artists can't be null");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is reqired");
            RuleFor(x => x.SongId).NotEmpty().WithMessage("SongId is reqired");
        }
    }
}
