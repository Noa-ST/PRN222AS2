using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenTuanKietRazorPages.Pages.Categoris
{
    [Authorize(Roles = "Staff")]
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CreateModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public Category Category { get; set; }

        public IActionResult OnGet()
        {
            Category = new Category(); // Khởi tạo với Articles rỗng
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"OnPostAsync - CategoryName: {Category?.CategoryName}, Status: {Category?.Status}");
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return Page();
            }

            // Tạo mới Category
            var newCategory = new Category
            {
                CategoryName = Category.CategoryName,
                Status = Category.Status,
                Articles = new List<NewsArticle>() // Đảm bảo Articles không null
            };

            try
            {
                await _categoryService.AddAsync(newCategory);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding category: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi thêm danh mục.");
                return Page();
            }
        }
    }
}