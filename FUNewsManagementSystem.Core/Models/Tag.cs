using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Core.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required(ErrorMessage = "Tên thẻ là bắt buộc")]
        [StringLength(255)]
        public string TagName { get; set; }

        public List<NewsArticleTag> NewsArticleTags { get; set; } = new();
    }
}
