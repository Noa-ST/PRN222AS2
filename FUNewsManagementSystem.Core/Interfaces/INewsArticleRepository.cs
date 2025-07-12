using FUNewsManagementSystem.Core.Models;


namespace FUNewsManagementSystem.Core.Interfaces
{
    public interface INewsArticleRepository
    {
        Task<List<NewsArticle>> GetActiveArticlesAsync();
        Task<NewsArticle> GetByIdAsync(int id);
        Task<List<NewsArticle>> GetAllAsync();
        Task AddAsync(NewsArticle article, int[] tagIds);
        Task UpdateAsync(NewsArticle article);
        Task DeleteAsync(int id);
        Task<List<NewsArticle>> SearchAsync(string title, int? categoryId, int? tagId);
        Task<List<NewsArticle>> GetByUserIdAsync(int userId);
    }
}
