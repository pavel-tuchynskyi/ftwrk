using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using Microsoft.AspNetCore.SignalR;

namespace FTWRK.Application.Hubs
{
    public class NotificationHub : Hub<INotificationClient>
    {
        public async Task ReportProgress(ProgressState state)
        {
            await Clients.Caller.ReportProgress(state);
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.GetConnectionId(Context.ConnectionId);
            await base.OnConnectedAsync();
        }
    }
}
