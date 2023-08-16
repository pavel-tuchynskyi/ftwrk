using AutoMapper;
using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Mappings;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Songs.AlbumSongs.Queries.GetSongArtists
{
    public class GetArtistsQuery : IRequest<PagedList<SongArtistDto>>, IMap<QueryParameters>
    {
        public FilterCondition FilterCondition { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetArtistsQuery, QueryParameters>()
                .ForMember(x => x.OrderBy, x => x.Ignore())
                .ForMember(x => x.Filter, x => x.MapFrom(y => new Filter()
                {
                    Operator = Operators.And,
                    Conditions = new List<FilterCondition>() { y.FilterCondition }
                }));
        }
    }
}
