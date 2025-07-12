using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FUNewsManagementSystem.Core.Models;
using FUNewsManagementSystem.Core.Interfaces;

namespace NguyenTuanKietRazorPages.Pages.Tags
{
    [Authorize(Roles = "Staff")]
    public class DeleteModel : PageModel
    {
        private readonly ITagService _tagService;

        public DeleteModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        [BindProperty]
        public Tag Tag { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Tag = await _tagService.GetByIdAsync(id);
            if (Tag == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                await _tagService.DeleteAsync(id);
                return RedirectToPage("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Tag = await _tagService.GetByIdAsync(id);
                return Page();
            }
        }
    }
}