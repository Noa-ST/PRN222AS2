
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using FUNewsManagementSystem.Core.Models;

namespace NguyenTuanKietRazorPages.Pages.NewsArticles
{
    [Authorize(Roles = "Staff")] // Cho phép cả Staff và Admin
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public DeleteModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Lấy id từ query string hoặc route data
            int? articleId = id ?? (RouteData.Values["id"] as int?);
            if (!articleId.HasValue)
            {
                Console.WriteLine("No valid ID provided for deletion");
                return BadRequest("ID is required");
            }

            Console.WriteLine($"Attempting to load Delete page for id: {articleId.Value}");
            NewsArticle = await _newsArticleService.GetByIdAsync(articleId.Value);
            if (NewsArticle == null)
            {
                Console.WriteLine($"Article with id {articleId.Value} not found in database");
                return NotFound($"Không tìm thấy bài viết với ID: {articleId.Value}");
            }
            Console.WriteLine($"Loaded article: {NewsArticle.Title} for deletion");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Console.WriteLine($"Attempting to delete article with id: {id}");
            var article = await _newsArticleService.GetByIdAsync(id);
            if (article != null)
            {
                try
                {
                    await _newsArticleService.DeleteAsync(id);
                    Console.WriteLine($"Successfully deleted article with id: {id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting article with id {id}: {ex.Message}");
                    return Page(); // Giữ trên trang để hiển thị lỗi
                }
            }
            else
            {
                Console.WriteLine($"Article with id {id} not found during deletion attempt");
            }
            return RedirectToPage("./Index");
        }
    }
}
