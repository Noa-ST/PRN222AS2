using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;

namespace FUNewsManagementSystem.Core.Services
{
    public class NewsArticleTagService : INewsArticleTagService
    {
        private readonly INewsArticleTagRepository _newsArticleTagRepository;

        public NewsArticleTagService(INewsArticleTagRepository newsArticleTagRepository)
        {
            _newsArticleTagRepository = newsArticleTagRepository;
        }

        public async Task AddAsync(NewsArticleTag newsArticleTag)
        {
            await _newsArticleTagRepository.AddAsync(newsArticleTag);
        }

        public async Task DeleteByArticleIdAsync(int articleId)
        {
            await _newsArticleTagRepository.DeleteByArticleIdAsync(articleId);
        }

        public async Task<List<NewsArticleTag>> GetByArticleIdAsync(int articleId)
        {
            return await _newsArticleTagRepository.GetByArticleIdAsync(articleId);
        }
    }
}
