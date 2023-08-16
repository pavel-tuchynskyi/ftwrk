using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Common.Interfaces
{
    public interface INotificationClient
    {
        Task ReportProgress(ProgressState state, string? error = null);
        Task GetConnectionId(string id);
    }
}
