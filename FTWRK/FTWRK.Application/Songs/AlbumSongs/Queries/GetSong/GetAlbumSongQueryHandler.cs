using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Songs;
using MediatR;

namespace FTWRK.Application.Songs.AlbumSongs.Queries.GetSong
{
    public class GetAlbumSongQueryHandler : IRequestHandler<GetAlbumSongQuery, SongBlob>
    {
        private readonly ISongBlobService _songBlobService;

        public GetAlbumSongQueryHandler(ISongBlobService songBlobService)
        {
            _songBlobService = songBlobService;
        }

        public async Task<SongBlob> Handle(GetAlbumSongQuery request, CancellationToken cancellationToken)
        {
            var songBlob = await _songBlobService.Get(request.SongId);

            return songBlob;
        }
    }
}
