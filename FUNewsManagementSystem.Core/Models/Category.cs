using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Core.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [StringLength(255)]
        public string CategoryName { get; set; }

        public int Status { get; set; } // 1=Active, 0=Inactive

        public List<NewsArticle> Articles { get; set; } = new List<NewsArticle>();
    }
}
