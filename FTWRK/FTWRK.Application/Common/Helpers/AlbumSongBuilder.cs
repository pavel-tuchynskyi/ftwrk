using AutoMapper;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Songs;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace FTWRK.Application.Common.Helpers
{
    public class AlbumSongBuilder
    {
        protected AlbumSong _albumSong;

        public AlbumSongBuilder() => _albumSong = new AlbumSong();

        protected AlbumSongBuilder(AlbumSong albumSong) => _albumSong = albumSong;

        public SongInfoBuilder SongInfo => new SongInfoBuilder(_albumSong);

        public SongBlobBuilder SongBlob => new SongBlobBuilder(_albumSong);

        public static implicit operator AlbumSong(AlbumSongBuilder builder)
        {
            return builder._albumSong;
        }
    }

    public class SongInfoBuilder : AlbumSongBuilder
    {
        public SongInfoBuilder(AlbumSong albumSong) : base(albumSong)
        {
            _albumSong = albumSong;
        }

        public SongInfoBuilder MapFromSource<T>(IMapper mapper, T source)
        {
            _albumSong = mapper.Map<AlbumSong>(source);
            return this;
        }

        public SongInfoBuilder SetDuration(TimeSpan duration)
        {
            _albumSong.Song.Duration = duration;
            return this;
        }
    }

    public class SongBlobBuilder : AlbumSongBuilder
    {
        public SongBlobBuilder(AlbumSong albumSong) : base(albumSong)
        {
            _albumSong = albumSong;
        }

        public SongBlobBuilder Create(byte[] songBytes, Guid? id = null)
        {
            if(id != null)
            {
                _albumSong.SongBlob.Id = (Guid)id;
            }
            else
            {
                _albumSong.SongBlob.Id = _albumSong.Song.Id;
            }

            _albumSong.SongBlob.SongBytes = songBytes;
            return this;
        }
    }
}
