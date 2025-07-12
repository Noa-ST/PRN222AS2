using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClosedXML.Excel;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NguyenTuanKietRazorPages.Pages
{
    [Authorize(Roles = "Admin")]
    public class ReportModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public ReportModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-30); // Mặc định 30 ngày trước
        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Today; // Mặc định hôm nay

        [BindProperty]
        public IList<NewsArticle> Articles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (StartDate > EndDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.");
                return Page();
            }

            Articles = await _newsArticleService.SearchAsync(null, null, null, StartDate, EndDate);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || StartDate > EndDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày không hợp lệ. Vui lòng kiểm tra lại.");
                return Page();
            }

            Articles = await _newsArticleService.SearchAsync(null, null, null, StartDate, EndDate);
            return Page();
        }

        public async Task<IActionResult> OnPostExportExcelAsync()
        {
            if (StartDate > EndDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày không hợp lệ. Vui lòng kiểm tra lại.");
                return Page();
            }

            Articles = await _newsArticleService.SearchAsync(null, null, null, StartDate, EndDate);

            // Tạo MemoryStream bên ngoài khối using
            var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");
                // Định nghĩa tiêu đề cho tất cả các cột
                worksheet.Cell("A1").Value = "Mã bài viết";
                worksheet.Cell("B1").Value = "Tiêu đề";
                worksheet.Cell("C1").Value = "Nội dung";
                worksheet.Cell("D1").Value = "Ngày tạo";
                worksheet.Cell("E1").Value = "Trạng thái";
                worksheet.Cell("F1").Value = "Mã danh mục";
                worksheet.Cell("G1").Value = "Người tạo";
                worksheet.Cell("H1").Value = "Người sửa";
                worksheet.Cell("I1").Value = "Ngày sửa";
                worksheet.Cell("J1").Value = "Tags";

                worksheet.Range("A1:J1").Style.Font.Bold = true;

                int row = 2;
                foreach (var article in Articles.OrderByDescending(a => a.CreatedDate))
                {
                    worksheet.Cell(row, 1).Value = article.ArticleId;
                    worksheet.Cell(row, 2).Value = article.Title;
                    worksheet.Cell(row, 3).Value = article.Content?.Substring(0, Math.Min(100, article.Content?.Length ?? 0)) ?? "N/A"; // Cắt ngắn nội dung
                    worksheet.Cell(row, 4).Value = article.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss");
                    worksheet.Cell(row, 5).Value = article.Status == 1 ? "Active" : "Inactive";
                    worksheet.Cell(row, 6).Value = article.CategoryId;
                    worksheet.Cell(row, 7).Value = article.CreatedBy ?? 0; // Giá trị mặc định nếu null
                    worksheet.Cell(row, 8).Value = article.ModifiedBy ?? 0; // Giá trị mặc định nếu null
                    worksheet.Cell(row, 9).Value = article.ModifiedDate?.ToString("dd/MM/yyyy HH:mm:ss") ?? "N/A";
                    // Lấy danh sách TagName từ NewsArticleTags
                    var tagNames = article.NewsArticleTags?.Select(t => t.Tag?.TagName).Where(t => t != null) ?? Enumerable.Empty<string>();
                    worksheet.Cell(row, 10).Value = string.Join(", ", tagNames);
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(stream); // Lưu vào stream
            }

            stream.Position = 0; // Đặt lại vị trí để đọc
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Report_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }
    }
}