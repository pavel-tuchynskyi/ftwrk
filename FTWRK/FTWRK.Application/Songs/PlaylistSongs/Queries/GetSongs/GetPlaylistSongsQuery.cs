using AutoMapper;
using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Mappings;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Songs.PlaylistSongs.Queries.GetPlaylistSongs
{
    public class GetPlaylistSongsQuery : IRequest<PagedList<SongDto>>, IMap<QueryParameters>
    {
        public Guid PlaylistId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetPlaylistSongsQuery, QueryParameters>();
        }
    }
}
