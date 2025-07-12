using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FUNewsManagementSystem.Core.Models;
using FUNewsManagementSystem.Core.Interfaces;
using System.Security.Claims;

namespace NguyenTuanKietRazorPages.Pages.NewsArticles
{
    public class CreateModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ITagRepository _tagRepo;

        public CreateModel(INewsArticleService newsArticleService, ICategoryRepository categoryRepo, ITagRepository tagRepo)
        {
            _newsArticleService = newsArticleService;
            _categoryRepo = categoryRepo;
            _tagRepo = tagRepo;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = new NewsArticle();

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new();

        public SelectList Categories { get; set; }
        public SelectList Tags { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"Received CategoryId: {NewsArticle.CategoryId}");
            Console.WriteLine($"User authenticated: {User.Identity?.IsAuthenticated ?? false}");

            if (NewsArticle.CategoryId <= 0)
            {
                ModelState.AddModelError("NewsArticle.CategoryId", "Vui lòng chọn một danh mục.");
                Console.WriteLine("CategoryId is invalid or not selected: " + NewsArticle.CategoryId);
            }

            NewsArticle.CreatedDate = DateTime.Now;
            NewsArticle.Status = 1;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            Console.WriteLine($"UserIdClaim: {userIdClaim?.Value ?? "null"}");
            if (userIdClaim != null)
            {
                if (int.TryParse(userIdClaim.Value, out int accountId))
                {
                    NewsArticle.CreatedBy = accountId;
                    Console.WriteLine($"Before AddAsync, CreatedBy set to: {NewsArticle.CreatedBy}");
                }
                else
                {
                    Console.WriteLine($"Invalid UserIdClaim value: {userIdClaim.Value}");
                    ModelState.AddModelError(string.Empty, "Giá trị UserId không hợp lệ.");
                }
            }
            else
            {
                Console.WriteLine("UserId claim not found");
                ModelState.AddModelError(string.Empty, "Không tìm thấy claim UserId.");
            }

            if (!ModelState.IsValid)
            {
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        Console.WriteLine($"Field {kvp.Key}: {error.ErrorMessage}");
                    }
                }
                await LoadDropdownsAsync();
                return Page();
            }

            try
            {
                await _newsArticleService.AddAsync(NewsArticle, SelectedTagIds?.ToArray() ?? new int[0]);
                Console.WriteLine($"Article added successfully, ArticleId: {NewsArticle.ArticleId}");
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding article: {ex.Message}");
                await LoadDropdownsAsync();
                return Page();
            }
        }

        private async Task LoadDropdownsAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            if (categories == null || !categories.Any())
            {
                ModelState.AddModelError(string.Empty, "Không có danh mục nào trong cơ sở dữ liệu.");
                Console.WriteLine("No categories found in database.");
            }
            var tags = await _tagRepo.GetAllAsync();

            Categories = new SelectList(categories, "CategoryId", "CategoryName");
            Tags = new SelectList(tags, "TagId", "TagName");
        }
    }
}
