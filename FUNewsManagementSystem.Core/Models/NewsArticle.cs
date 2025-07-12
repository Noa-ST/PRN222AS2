using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagementSystem.Core.Models
{
    public class NewsArticle
    {
        [Key]
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [StringLength(255)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Nội dung là bắt buộc")]
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Status { get; set; } // 1=Active, 0=Inactive

        [Required(ErrorMessage = "Danh mục là bắt buộc")]
        public int CategoryId { get; set; }
        //public Category Category { get; set; }

        public int? CreatedBy { get; set; } // Thay đổi thành nullable int
        // Loại bỏ ForeignKey("Creator") để tránh yêu cầu đối tượng Account đầy đủ
        // public Account Creator { get; set; }

        public int? ModifiedBy { get; set; } // Thêm trường này
        public DateTime? ModifiedDate { get; set; } // Thêm trường này

        public List<NewsArticleTag> NewsArticleTags { get; set; } = new(); // Khởi tạo mặc định để tránh null
    }
}
