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
        [BindProperty(SupportsGet = true)]
        public string? ConnectionId { get; set; }

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
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int accountId))
            {
                NewsArticle.CreatedBy = accountId;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không xác định được người dùng.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            try
            {
                // Lấy connectionId từ form (gửi bằng JS thông qua input ẩn)
                var connectionId = Request.Form["connectionId"].ToString();

                await _newsArticleService.AddAsync(NewsArticle, SelectedTagIds?.ToArray() ?? Array.Empty<int>(), connectionId);
                return RedirectToPage("/NewsArticles/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo bài viết.");
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
