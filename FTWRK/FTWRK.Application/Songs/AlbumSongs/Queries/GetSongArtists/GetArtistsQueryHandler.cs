using AutoMapper;
using FTWRK.Application.Common.Constants;
using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Songs.AlbumSongs.Queries.GetSongArtists
{
    public class GetArtistsQueryHandler : IRequestHandler<GetArtistsQuery, PagedList<SongArtistDto>>
    {
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        private readonly IMapper _mapper;

        public GetArtistsQueryHandler(IUserManager userManager, IRoleManager roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<PagedList<SongArtistDto>> Handle(GetArtistsQuery request, CancellationToken cancellationToken)
        {
            var parameters = _mapper.Map<QueryParameters>(request);
            var role = await _roleManager.GetRoleByName(RoleNames.Artist);
            var roleCondition = new FilterCondition
            {
                ConditionType = FilterConditionType.InArray,
                Key = nameof(IUser.Roles),
                Value = role.Id
            };

            parameters.Filter.Conditions.Add(roleCondition);

            var users = await _userManager.GetUsers(parameters);

            var artistsDto = _mapper.Map<PagedList<SongArtistDto>>(users);

            return artistsDto;
        }
    }
}
