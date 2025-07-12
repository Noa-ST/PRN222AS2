using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Hubs
{
    public class NewsHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}