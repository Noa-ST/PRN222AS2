// Pages/NewsArticles/Details.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NguyenTuanKietRazorPages.Pages.NewsArticles
{
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _userService;

        public DetailsModel(INewsArticleService newsArticleService, ICategoryService categoryService, IAccountService userService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _userService = userService;
        }

        public NewsArticle NewsArticle { get; set; }
        public SelectList Categories { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public List<string> TagNames { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            NewsArticle = await _newsArticleService.GetByIdAsync(id);
            if (NewsArticle == null)
            {
                return NotFound();
            }

            if (NewsArticle.CreatedBy.HasValue)
            {
                var creator = await _userService.GetByIdAsync(NewsArticle.CreatedBy.Value);
                CreatedByName = creator?.FullName ?? "Chưa xác định";
            }

            if (NewsArticle.ModifiedBy.HasValue && NewsArticle.ModifiedDate.HasValue)
            {
                var modifier = await _userService.GetByIdAsync(NewsArticle.ModifiedBy.Value);
                ModifiedByName = modifier?.FullName ?? "Chưa xác định";
            }

            Categories = new SelectList(await _categoryService.GetAllAsync(), "CategoryId", "CategoryName");
            TagNames = NewsArticle.NewsArticleTags?.Select(t => t.Tag?.TagName).Where(name => !string.IsNullOrEmpty(name)).ToList() ?? new();

            return Page();
        }
    }
}
