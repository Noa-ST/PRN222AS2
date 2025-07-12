using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenTuanKietRazorPages.Pages.Accounts
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IAccountService _accountService;

        public EditModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public Account Account { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Account = await _accountService.GetByIdAsync(id);
            if (Account == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Mã hóa mật khẩu nếu có thay đổi (không để trống)
            if (!string.IsNullOrEmpty(Account.Password))
            {
                Account.Password = BCrypt.Net.BCrypt.HashPassword(Account.Password);
            }
            await _accountService.UpdateAsync(Account);
            return RedirectToPage("./Index");
        }
    }
}
