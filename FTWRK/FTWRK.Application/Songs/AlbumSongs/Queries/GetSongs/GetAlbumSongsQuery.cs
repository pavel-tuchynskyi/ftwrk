using AutoMapper;
using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Mappings;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Songs.AlbumSongs.Queries.GetSongs
{
    public class GetAlbumSongsQuery : IRequest<PagedList<SongDto>>, IMap<QueryParameters>
    {
        public Guid? AlbumId { get; set; }
        public Filter? Filter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetAlbumSongsQuery, QueryParameters>();
        }
    }
}
