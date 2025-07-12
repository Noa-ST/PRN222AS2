using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NguyenTuanKietRazorPages.Pages.Dashboard
{
    [Authorize(Roles = "Staff")]
    public class StaffModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public StaffModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        public IList<NewsArticle> Articles { get; set; }
        public SelectList Categories { get; set; }
        public IList<Tag> Tags { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Articles = await _newsArticleService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();
            Categories = new SelectList(categories, "CategoryId", "CategoryName");
            Tags = await _tagService.GetAllAsync();
            return Page();
        }
    }
}
