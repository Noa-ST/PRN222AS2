using FUNewsManagementSystem.Hubs;
using FUNewsManagementSystem.Services;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NewsHub> _hubContext;

        public NotificationService(IHubContext<NewsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyNewArticleAsync(string articleTitle)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Bài viết mới: {articleTitle}");
        }

        public async Task NotifyArticleUpdatedAsync(string articleTitle)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Bài viết được cập nhật: {articleTitle}");
        }
    }
}