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
     

        public int? CreatedBy { get; set; } 
   
        public int? ModifiedBy { get; set; } 
        public DateTime? ModifiedDate { get; set; } 

        public List<NewsArticleTag> NewsArticleTags { get; set; } = new(); 
    }
}
