using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace NguyenTuanKietRazorPages.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;

        public IndexModel(INewsArticleService newsArticleService, ICategoryService categoryService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTitle { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? SearchCategoryId { get; set; }

        public IList<NewsArticle> Articles { get; set; }
        public SelectList Categories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy danh sách danh mục
            Categories = new SelectList(await _categoryService.GetAllAsync(), "CategoryId", "CategoryName");

            // Lấy tất cả bài viết làm mặc định
            Articles = await _newsArticleService.GetActiveArticlesAsync();
            Console.WriteLine($"Default articles count from GetActiveArticlesAsync: {Articles?.Count ?? 0}");

            // Áp dụng tìm kiếm nếu có điều kiện
            if (!string.IsNullOrEmpty(SearchTitle) || SearchCategoryId.HasValue)
            {
                Console.WriteLine($"Searching with Title: '{SearchTitle}', CategoryId: {SearchCategoryId}");
                Articles = await _newsArticleService.SearchAsync(SearchTitle, SearchCategoryId, null);
                Console.WriteLine($"Search results count: {Articles?.Count ?? 0}");
            }

            return Page();
        }
    }
}