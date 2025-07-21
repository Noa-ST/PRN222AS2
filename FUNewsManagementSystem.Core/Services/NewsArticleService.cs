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

        public async Task AddAsync(NewsArticle article, int[] tagIds, string? connectionId = null)
        {
            article.CreatedDate = DateTime.Now;
            article.Status = 1;

            var userId = GetCurrentUserId();
            if (userId.HasValue)
            {
                article.CreatedBy = userId.Value;
            }

            await _newsArticleRepository.AddAsync(article, tagIds);
            await _notificationService.NotifyNewArticleAsync(article.Title, article.ArticleId, connectionId);
        }

        public async Task UpdateAsync(NewsArticle article, int[] tagIds, string? connectionId = null)
        {
            var userId = GetCurrentUserId();
            if (userId.HasValue)
            {
                article.ModifiedBy = userId.Value;
                article.ModifiedDate = DateTime.Now;

                if (!article.CreatedBy.HasValue || article.CreatedBy == 0)
                {
                    article.CreatedBy = userId.Value;
                }
            }

            await _newsArticleRepository.UpdateAsync(article);
            await _newsArticleTagRepository.DeleteByArticleIdAsync(article.ArticleId);

            foreach (var tagId in tagIds)
            {
                await _newsArticleTagRepository.AddAsync(new NewsArticleTag
                {
                    ArticleId = article.ArticleId,
                    TagId = tagId
                });
            }

            await _notificationService.NotifyArticleUpdatedAsync(article.Title, article.ArticleId, connectionId);
        }

        // Helper để lấy userId
        private int? GetCurrentUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out int userId))
            {
                return userId;
            }
            return null;
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
