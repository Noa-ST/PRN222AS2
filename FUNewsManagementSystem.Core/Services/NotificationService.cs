using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Hubs;
using FUNewsManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FUNewsManagementSystem.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NewsHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _accountService;

        public NotificationService(
            IHubContext<NewsHub> hubContext,
            IHttpContextAccessor httpContextAccessor,
            IAccountService accountService)
        {
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
        }

        public async Task NotifyNewArticleAsync(string title, int articleId, string? excludeConnectionId = null)
        {
            var message = await BuildMessageAsync("đã thêm bài viết mới", title);
            await SendNotificationAsync(message, articleId, excludeConnectionId);
        }

        public async Task NotifyArticleUpdatedAsync(string title, int articleId, string? excludeConnectionId = null)
        {
            var message = await BuildMessageAsync("đã cập nhật bài viết", title);
            await SendNotificationAsync(message, articleId, excludeConnectionId);
        }

        private async Task<string> BuildMessageAsync(string action, string title)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null && int.TryParse(userId, out int accountId))
            {
                var account = await _accountService.GetByIdAsync(accountId);
                var fullName = account?.FullName ?? "Người dùng";
                return $"{fullName} {action}: {title}";
            }

            return $"Người dùng {action}: {title}";
        }

        private async Task SendNotificationAsync(string message, int articleId, string? excludeConnectionId)
        {
            if (!string.IsNullOrEmpty(excludeConnectionId))
            {
                await _hubContext.Clients
                    .AllExcept(excludeConnectionId)
                    .SendAsync("ReceiveNotification", new
                    {
                        Message = message,
                        ArticleId = articleId
                    });
            }
            else
            {
                await _hubContext.Clients
                    .All
                    .SendAsync("ReceiveNotification", new
                    {
                        Message = message,
                        ArticleId = articleId
                    });
            }
        }
    }
}
