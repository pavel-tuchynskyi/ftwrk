using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Albums;
using MediatR;

namespace FTWRK.Application.Recommendations.Queries.GetUserRecommendations
{
    public class GetUserRecommendationsQueryHandler : IRequestHandler<GetUserRecommendationsQuery, List<Album>>
    {
        private readonly IUserAnalyticsService _analytics;
        private readonly IRecommendationService _recommendations;
        private readonly Guid _userId;

        public GetUserRecommendationsQueryHandler(IUserAnalyticsService analytics, 
            IRecommendationService recommendations, IUserContextService userContext)
        {
            _analytics = analytics;
            _recommendations = recommendations;
            _userId = userContext.GetUserId();
        }
        public async Task<List<Album>> Handle(GetUserRecommendationsQuery request, CancellationToken cancellationToken)
        {
            var userHistory = await _analytics.GetById(_userId);
            var albums = userHistory.SelectMany(x => x.Albums).ToList();
            var genres = userHistory.Select(x => x.Genre).ToList();

            var recommendations = await _recommendations.GetRecommendations(_userId, albums, genres);

            return recommendations;
        }
    }
}
