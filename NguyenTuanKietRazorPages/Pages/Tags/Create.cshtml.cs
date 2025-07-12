using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FUNewsManagementSystem.Core.Models;
using FUNewsManagementSystem.Core.Interfaces;

namespace NguyenTuanKietRazorPages.Pages.Tags
{
    [Authorize(Roles = "Staff")]
    public class CreateModel : PageModel
    {
        private readonly ITagService _tagService;

        public CreateModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        [BindProperty]
        public Tag Tag { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _tagService.AddAsync(Tag);
            return RedirectToPage("Index");
        }
    }
}
