using FTWRK.Application.Common.DTO.Analytics;
using FTWRK.Domain.Entities.Analythics;

namespace FTWRK.Application.Common.Interfaces
{
    public interface IUserAnalyticsService
    {
        Task<List<ListeningHistoryDTO>> GetById(Guid userId);
        Task<bool> Add(ListeningHistory history);
    }
}
