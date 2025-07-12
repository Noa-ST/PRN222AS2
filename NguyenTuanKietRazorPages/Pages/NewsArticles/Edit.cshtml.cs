
  using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FUNewsManagementSystem.Core.Models;
using FUNewsManagementSystem.Core.Interfaces;
using System.Threading.Tasks;
using System.Security.Claims;

namespace NguyenTuanKietRazorPages.Pages.NewsArticles
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ITagRepository _tagRepo;

        public EditModel(INewsArticleService newsArticleService, ICategoryRepository categoryRepo, ITagRepository tagRepo)
        {
            _newsArticleService = newsArticleService;
            _categoryRepo = categoryRepo;
            _tagRepo = tagRepo;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new();

        public SelectList Categories { get; set; }
        public SelectList Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            NewsArticle = await _newsArticleService.GetByIdAsync(id);
            if (NewsArticle == null)
            {
                return NotFound();
            }
            await LoadDropdownsAsync();
            SelectedTagIds = NewsArticle.NewsArticleTags?.Select(t => t.TagId).ToList() ?? new List<int>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, int[] tagIds)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            NewsArticle = await _newsArticleService.GetByIdAsync(id);
            if (NewsArticle == null)
            {
                return NotFound();
            }

            // Cập nhật các trường từ form
            NewsArticle.CategoryId = NewsArticle.CategoryId; // Giữ nguyên hoặc cập nhật từ form nếu có
            NewsArticle.Title = NewsArticle.Title; // Cập nhật từ form
            NewsArticle.Content = NewsArticle.Content; // Cập nhật từ form
            NewsArticle.Status = NewsArticle.Status; // Giữ nguyên hoặc cập nhật từ form

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int accountId))
            {
                NewsArticle.ModifiedBy = accountId;
                NewsArticle.ModifiedDate = DateTime.Now;
                if (!NewsArticle.CreatedBy.HasValue)
                {
                    NewsArticle.CreatedBy = accountId;
                }
            }

            // Cập nhật NewsArticleTags
            NewsArticle.NewsArticleTags = tagIds?.Select(tid => new NewsArticleTag { ArticleId = id, TagId = tid }).ToList() ?? new List<NewsArticleTag>();

            await _newsArticleService.UpdateAsync(NewsArticle, tagIds);
            return RedirectToPage("./Index");
        }

        private async Task LoadDropdownsAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            var tags = await _tagRepo.GetAllAsync();

            Categories = new SelectList(categories, "CategoryId", "CategoryName");
            Tags = new SelectList(tags, "TagId", "TagName");
        }
    }
}
