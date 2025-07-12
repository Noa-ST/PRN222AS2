using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenTuanKietRazorPages.Pages.Categoris
{
    [Authorize(Roles = "Staff")]
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public EditModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"OnPostAsync - CategoryId: {Category?.CategoryId}, CategoryName: {Category?.CategoryName}, Status: {Category?.Status}");
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return Page();
            }

            var existingCategory = await _categoryService.GetByIdAsync(Category.CategoryId);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.CategoryName = Category.CategoryName;
            existingCategory.Status = Category.Status;

            try
            {
                await _categoryService.UpdateAsync(existingCategory);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi cập nhật danh mục.");
                return Page();
            }
        }
    }
}