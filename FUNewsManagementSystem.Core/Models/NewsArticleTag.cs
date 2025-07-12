using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagementSystem.Core.Models
{
    public class NewsArticleTag
    {
        [Key]
        [Column(Order = 1)]
        public int ArticleId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int TagId { get; set; }

        public NewsArticle Article { get; set; }
        public Tag Tag { get; set; }
    }
}
