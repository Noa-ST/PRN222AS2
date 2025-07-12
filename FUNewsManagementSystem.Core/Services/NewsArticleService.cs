using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using FUNewsManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FUNewsManagementSystem.Core.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly INewsArticleTagRepository _newsArticleTagRepository;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NewsArticleService(
            INewsArticleRepository newsArticleRepository,
            INewsArticleTagRepository newsArticleTagRepository,
            INotificationService notificationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _newsArticleRepository = newsArticleRepository;
            _newsArticleTagRepository = newsArticleTagRepository;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<NewsArticle>> GetActiveArticlesAsync()
        {
            return await _newsArticleRepository.GetActiveArticlesAsync();
        }

        public async Task<NewsArticle> GetByIdAsync(int id)
        {
            return await _newsArticleRepository.GetByIdAsync(id);
        }

        public async Task<List<NewsArticle>> GetAllAsync()
        {
            return await _newsArticleRepository.GetAllAsync();
        }

        public async Task AddAsync(NewsArticle article, int[] tagIds)
        {
            Console.WriteLine($"In AddAsync, Received CreatedBy: {article.CreatedBy}");
            article.CreatedDate = DateTime.Now;
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            int accountId = 0;
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out accountId))
            {
                Console.WriteLine($"User ID from claim: {accountId}");
                // Chỉ gán nếu article.CreatedBy chưa được thiết lập
                if (!article.CreatedBy.HasValue)
                {
                    article.CreatedBy = accountId;
                }
            }
            else
            {
                Console.WriteLine("No valid user ID found in HttpContext");
            }
            article.Status = 1;
            await _newsArticleRepository.AddAsync(article, tagIds);

            await _notificationService.NotifyNewArticleAsync(article.Title);
        }


        public async Task UpdateAsync(NewsArticle article, int[] tagIds)
        {
            Console.WriteLine($"In UpdateAsync, Received CreatedBy: {article.CreatedBy}, ModifiedBy: {article.ModifiedBy}");
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int accountId))
            {
                Console.WriteLine($"User ID from claim: {accountId}");
                // Gán ModifiedBy
                article.ModifiedBy = accountId;
                article.ModifiedDate = DateTime.Now;
                // Giữ nguyên CreatedBy nếu đã có, chỉ gán nếu null
                if (!article.CreatedBy.HasValue && article.CreatedBy != 0)
                {
                    article.CreatedBy = accountId;
                }
            }
            else
            {
                Console.WriteLine("No valid user ID found in HttpContext");
            }

            await _newsArticleRepository.UpdateAsync(article);

            await _newsArticleTagRepository.DeleteByArticleIdAsync(article.ArticleId);
            foreach (var tagId in tagIds)
            {
                await _newsArticleTagRepository.AddAsync(new NewsArticleTag { ArticleId = article.ArticleId, TagId = tagId });
            }

            await _notificationService.NotifyArticleUpdatedAsync(article.Title);
        }

        public async Task DeleteAsync(int id)
        {
            await _newsArticleTagRepository.DeleteByArticleIdAsync(id);
            await _newsArticleRepository.DeleteAsync(id);
        }

        public async Task<List<NewsArticle>> SearchAsync(string title, int? categoryId, int? tagId)
        {
            return await _newsArticleRepository.SearchAsync(title, categoryId, tagId);
        }

        public async Task<IList<NewsArticle>> SearchAsync(string title, int? categoryId, int? tagId, DateTime? startDate, DateTime? endDate)
        {
            var articles = await _newsArticleRepository.GetAllAsync();
            return articles.Where(a =>
                (string.IsNullOrEmpty(title) || a.Title.Contains(title)) &&
                (!categoryId.HasValue || a.CategoryId == categoryId) &&
                (!tagId.HasValue || a.NewsArticleTags.Any(t => t.TagId == tagId)) &&
                (!startDate.HasValue || a.CreatedDate >= startDate) &&
                (!endDate.HasValue || a.CreatedDate <= endDate)
            ).ToList();
        }

        public async Task<List<NewsArticle>> GetByUserIdAsync(int userId)
        {
            return await _newsArticleRepository.GetByUserIdAsync(userId);
        }
    }
}
