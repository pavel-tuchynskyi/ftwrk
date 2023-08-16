using FTWRK.Domain.Entities.Albums;

namespace FTWRK.Application.Common.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<Album>> GetRecommendations(Guid userId, List<Guid> userAlbums, List<string> genres);
    }
}
