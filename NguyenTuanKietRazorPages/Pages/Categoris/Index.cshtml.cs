using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenTuanKietRazorPages.Pages.Categoris
{
    [Authorize(Roles = "Staff")]
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleService _newsArticleService;

        public IndexModel(ICategoryService categoryService, INewsArticleService newsArticleService)
        {
            _categoryService = categoryService;
            _newsArticleService = newsArticleService;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? SearchStatus { get; set; }

        public IList<Category> Categories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Console.WriteLine($"OnGetAsync - SearchName: {SearchName}, SearchStatus: {SearchStatus}");
            Categories = await _categoryService.SearchAsync(SearchName, SearchStatus);
            return Page();
        }
    }
}