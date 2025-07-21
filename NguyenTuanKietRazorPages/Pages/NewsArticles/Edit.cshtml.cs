using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FUNewsManagementSystem.Core.Models;
using FUNewsManagementSystem.Core.Interfaces;
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
            if (NewsArticle == null) return NotFound();

            SelectedTagIds = NewsArticle.NewsArticleTags?.Select(t => t.TagId).ToList() ?? new List<int>();

            await LoadDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            var existingArticle = await _newsArticleService.GetByIdAsync(id);
            if (existingArticle == null) return NotFound();

            // Cập nhật thông tin
            existingArticle.Title = NewsArticle.Title;
            existingArticle.Content = NewsArticle.Content;
            existingArticle.CategoryId = NewsArticle.CategoryId;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                existingArticle.ModifiedBy = userId;
                existingArticle.ModifiedDate = DateTime.Now;
            }

            await _newsArticleService.UpdateAsync(existingArticle, SelectedTagIds.ToArray());
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
