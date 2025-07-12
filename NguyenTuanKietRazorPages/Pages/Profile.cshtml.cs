using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace NguyenTuanKietRazorPages.Pages
{
    [Authorize(Roles = "Staff")]
    public class ProfileModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly INewsArticleService _newsArticleService;

        public ProfileModel(IAccountService accountService, INewsArticleService newsArticleService)
        {
            _accountService = accountService;
            _newsArticleService = newsArticleService;
        }

        [BindProperty]
        public Account Account { get; set; }

        public IList<NewsArticle> Articles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return NotFound("Không tìm thấy ID người dùng.");
            }

            Account = await _accountService.GetByIdAsync(userId);
            if (Account == null)
            {
                return NotFound();
            }

            Articles = await _newsArticleService.GetByUserIdAsync(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _accountService.UpdateAsync(Account);
            return RedirectToPage();
        }
    }
}
