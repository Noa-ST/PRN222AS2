using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenTuanKietRazorPages.Pages.Categoris
{
    [Authorize(Roles = "Staff")]
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleService _newsArticleService;

        public DeleteModel(ICategoryService categoryService, INewsArticleService newsArticleService)
        {
            _categoryService = categoryService;
            _newsArticleService = newsArticleService;
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _categoryService.GetByIdAsync(id);
            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var articles = await _newsArticleService.GetAllAsync();
            if (articles.Any(a => a.CategoryId == id))
            {
                ModelState.AddModelError(string.Empty, "Không thể xóa danh mục vì có bài viết liên kết.");
                return Page();
            }

            await _categoryService.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
    }
}
