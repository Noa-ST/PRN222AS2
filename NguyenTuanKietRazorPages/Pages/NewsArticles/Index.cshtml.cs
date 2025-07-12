using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using FUNewsManagementSystem.Hubs;

namespace NguyenTuanKietRazorPages.Pages.NewsArticles
{
    [Authorize(Roles = "Staff")]
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IHubContext<NewsHub> _hubContext;

        public IndexModel(INewsArticleService newsArticleService, ICategoryService categoryService,
            ITagService tagService, IHubContext<NewsHub> hubContext)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _hubContext = hubContext;
        }

        [BindProperty(SupportsGet = true)] public string SearchTitle { get; set; }
        [BindProperty(SupportsGet = true)] public int? SearchCategoryId { get; set; }
        [BindProperty(SupportsGet = true)] public int? SearchTagId { get; set; }
        [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = 1;

        public const int PageSize = 6;
        public IList<NewsArticle> Articles { get; set; } = new List<NewsArticle>();
        public SelectList Categories { get; set; }
        public SelectList Tags { get; set; }
        public int TotalPages { get; set; }

        // Used for binding forms
        [BindProperty] public NewsArticle NewArticle { get; set; }
        [BindProperty] public NewsArticle EditArticle { get; set; }
        [BindProperty] public int[] SelectedTagIds { get; set; } = new int[0];

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                return Page();
            }

            await _newsArticleService.AddAsync(NewArticle, SelectedTagIds);
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", "created", NewArticle.Title);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                return Page();
            }

            EditArticle.ArticleId = id;
            await _newsArticleService.UpdateAsync(EditArticle, SelectedTagIds);
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", "updated", EditArticle.Title);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var article = await _newsArticleService.GetByIdAsync(id);
            if (article != null)
            {
                await _newsArticleService.DeleteAsync(id);
                await _hubContext.Clients.All.SendAsync("ReceiveUpdate", "deleted", article.Title);
            }
            return RedirectToPage();
        }

        private async Task LoadDataAsync()
        {
            Categories = new SelectList(await _categoryService.GetAllAsync(), "CategoryId", "CategoryName");
            Tags = new SelectList(await _tagService.GetAllAsync(), "TagId", "TagName");

            var allArticles = await _newsArticleService.SearchAsync(SearchTitle, SearchCategoryId, SearchTagId);
            TotalPages = (int)Math.Ceiling(allArticles.Count / (double)PageSize);
            Articles = allArticles.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}
