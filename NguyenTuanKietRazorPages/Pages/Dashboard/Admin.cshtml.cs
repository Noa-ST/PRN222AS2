using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;

namespace NguyenTuanKietRazorPages.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class AdminModel : PageModel
    {
        public IActionResult OnGet()
        {
            Console.WriteLine($"Admin page loaded - IsAuthenticated: {User.Identity.IsAuthenticated}, Role: {User.FindFirst(ClaimTypes.Role)?.Value}, Claims Count: {User.Claims.Count()}");
            return Page();
        }

        public IActionResult OnPostManageAccounts()
        {
            return RedirectToPage("/Accounts/Index");
        }

        public IActionResult OnPostManageReports()
        {
            // Đặt ngày mặc định trong backend
            DateTime startDate = DateTime.Today.AddDays(-30); // 30 ngày trước
            DateTime endDate = DateTime.Today; // Hôm nay
            return RedirectToPage("/Report", new { startDate, endDate });
        }
    }
}