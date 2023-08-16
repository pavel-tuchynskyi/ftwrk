using FTWRK.Domain.Entities.Albums;
using MediatR;

namespace FTWRK.Application.Recommendations.Queries.GetUserRecommendations
{
    public class GetUserRecommendationsQuery : IRequest<List<Album>>
    {
    }
}
