

using FUNewsManagementSystem.Core.Models;

namespace FUNewsManagementSystem.Core.Interfaces
{
    public interface INewsArticleService
    {
        Task<List<NewsArticle>> GetActiveArticlesAsync();
        Task<NewsArticle> GetByIdAsync(int id);
        Task<List<NewsArticle>> GetAllAsync();
        Task AddAsync(NewsArticle article, int[] tagIds);
        Task UpdateAsync(NewsArticle article, int[] tagIds);
        Task DeleteAsync(int id);
        Task<List<NewsArticle>> SearchAsync(string title, int? categoryId, int? tagId);
        Task<IList<NewsArticle>> SearchAsync(string title, int? categoryId, int? tagId, DateTime? startDate, DateTime? endDate); 

        Task<List<NewsArticle>> GetByUserIdAsync(int userId);
    }
}
