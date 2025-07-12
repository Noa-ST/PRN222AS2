using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;

namespace NguyenTuanKietRazorPages.Pages.Tags
{
    [Authorize(Roles = "Staff")]
    public class IndexModel : PageModel
    {
        private readonly ITagService _tagService;

        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IList<Tag> Tags { get; set; }

        public async Task OnGetAsync()
        {
            Tags = await _tagService.GetAllAsync();
        }
    }
}
