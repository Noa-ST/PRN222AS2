using FUNewsManagementSystem.Core.Models;


namespace FUNewsManagementSystem.Core.Interfaces
{
    public interface INewsArticleTagRepository
    {
        Task AddAsync(NewsArticleTag newsArticleTag);
        Task DeleteByArticleIdAsync(int articleId);
        Task<List<NewsArticleTag>> GetByArticleIdAsync(int articleId);
    }
}

