// Pages/Tags/Edit.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FUNewsManagementSystem.Core.Models;
using FUNewsManagementSystem.Core.Interfaces;

namespace NguyenTuanKietRazorPages.Pages.Tags
{
    [Authorize(Roles = "Staff")]
    public class EditModel : PageModel
    {
        private readonly ITagService _tagService;

        public EditModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        [BindProperty]
        public Tag Tag { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Tag = await _tagService.GetByIdAsync(id.Value);
            if (Tag == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var existingTag = await _tagService.GetByIdAsync(Tag.TagId);
            if (existingTag == null) return NotFound();

            existingTag.TagName = Tag.TagName;
            await _tagService.UpdateAsync(existingTag);

            return RedirectToPage("Index");
        }
    }
}
