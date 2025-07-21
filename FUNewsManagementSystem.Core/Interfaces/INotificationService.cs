namespace FUNewsManagementSystem.Services
{
    public interface INotificationService
    {
        Task NotifyNewArticleAsync(string title, int articleId, string? excludeConnectionId = null);
        Task NotifyArticleUpdatedAsync(string title, int articleId, string? excludeConnectionId = null);
    }
}
