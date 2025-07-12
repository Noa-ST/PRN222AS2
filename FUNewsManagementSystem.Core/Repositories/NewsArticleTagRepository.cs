using FUNewsManagementSystem.Core.Data;
using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Core.Repositories
{
    public class NewsArticleTagRepository : INewsArticleTagRepository
    {
        private readonly FUNewsManagementContext _context;

        public NewsArticleTagRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NewsArticleTag newsArticleTag)
        {
            await _context.NewsArticleTags.AddAsync(newsArticleTag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByArticleIdAsync(int articleId)
        {
            var tags = await _context.NewsArticleTags.Where(nat => nat.ArticleId == articleId).ToListAsync();
            _context.NewsArticleTags.RemoveRange(tags);
            await _context.SaveChangesAsync();
        }

        public async Task<List<NewsArticleTag>> GetByArticleIdAsync(int articleId)
        {
            return await _context.NewsArticleTags
                .Include(nat => nat.Tag)
                .Where(nat => nat.ArticleId == articleId)
                .ToListAsync();
        }
    }
}
