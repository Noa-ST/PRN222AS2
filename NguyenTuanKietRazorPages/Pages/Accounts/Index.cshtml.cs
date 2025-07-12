using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenTuanKietRazorPages.Pages.Accounts
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchEmail { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? SearchRole { get; set; }

        public IList<Account> Accounts { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Accounts = await _accountService.SearchAsync(SearchEmail, SearchRole);
            return Page();
        }
    }
}
