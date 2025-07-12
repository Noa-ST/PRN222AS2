
  using FUNewsManagementSystem.Core.Data;
using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Core.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FUNewsManagementContext _context;

        public NewsArticleRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public async Task<List<NewsArticle>> GetActiveArticlesAsync() =>
            await _context.NewsArticles
                .Include(a => a.NewsArticleTags)
                .ThenInclude(nat => nat.Tag)
                .Where(a => a.Status == 1)
                .ToListAsync();

        public async Task<NewsArticle> GetByIdAsync(int id) =>
            await _context.NewsArticles
                .Include(a => a.NewsArticleTags)
                .ThenInclude(nat => nat.Tag)
                .FirstOrDefaultAsync(a => a.ArticleId == id);

        public async Task<List<NewsArticle>> GetAllAsync() =>
            await _context.NewsArticles
                .Include(a => a.NewsArticleTags)
                .ThenInclude(nat => nat.Tag)
                .ToListAsync();

        public async Task AddAsync(NewsArticle article, int[] tagIds)
        {
            Console.WriteLine($"In Repository AddAsync, Received CreatedBy: {article.CreatedBy}");
            await _context.NewsArticles.AddAsync(article);
            await _context.SaveChangesAsync();

            if (tagIds?.Length > 0)
            {
                var tags = tagIds.Select(tagId => new NewsArticleTag { ArticleId = article.ArticleId, TagId = tagId }).ToList();
                await _context.NewsArticleTags.AddRangeAsync(tags);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(NewsArticle article)
        {
            Console.WriteLine($"In Repository UpdateAsync, Received CreatedBy: {article.CreatedBy}, ModifiedBy: {article.ModifiedBy}");
            var existingTags = await _context.NewsArticleTags
                .Where(t => t.ArticleId == article.ArticleId)
                .ToListAsync();

            // Xóa các tags không còn trong tagIds
            var tagsToRemove = existingTags
                .Where(t => !article.NewsArticleTags.Any(nt => nt.TagId == t.TagId))
                .ToList();
            if (tagsToRemove.Any())
            {
                _context.NewsArticleTags.RemoveRange(tagsToRemove);
            }

            // Thêm các tags mới
            var newTags = article.NewsArticleTags
                .Where(nt => !existingTags.Any(t => t.TagId == nt.TagId))
                .ToList();
            if (newTags.Any())
            {
                await _context.NewsArticleTags.AddRangeAsync(newTags);
            }

            _context.NewsArticles.Update(article);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var article = await _context.NewsArticles.FindAsync(id);
            if (article != null)
            {
                var relatedTags = _context.NewsArticleTags.Where(t => t.ArticleId == id);
                _context.NewsArticleTags.RemoveRange(relatedTags);
                _context.NewsArticles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<NewsArticle>> SearchAsync(string title, int? categoryId, int? tagId)
        {
            var query = _context.NewsArticles
                .Include(a => a.NewsArticleTags)
                .ThenInclude(nat => nat.Tag)
                .AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(a => a.Title.Contains(title));
            if (categoryId.HasValue)
                query = query.Where(a => a.CategoryId == categoryId.Value);
            if (tagId.HasValue)
                query = query.Where(a => a.NewsArticleTags.Any(nat => nat.TagId == tagId.Value));

            return await query.ToListAsync();
        }

        public async Task<List<NewsArticle>> GetByUserIdAsync(int userId) =>
            await _context.NewsArticles
                .Include(a => a.NewsArticleTags)
                .ThenInclude(nat => nat.Tag)
                .Where(a => a.CreatedBy == userId)
                .ToListAsync();
    }
}
