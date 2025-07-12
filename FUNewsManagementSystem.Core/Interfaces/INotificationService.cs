namespace FUNewsManagementSystem.Services
{
    public interface INotificationService
    {
        Task NotifyNewArticleAsync(string articleTitle);
        Task NotifyArticleUpdatedAsync(string articleTitle);
    }
}
