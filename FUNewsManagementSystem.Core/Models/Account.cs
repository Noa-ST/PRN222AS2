using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Core.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int Role { get; set; } // 0=Admin, 1=Staff, 2=Lecturer

        [StringLength(255)]
        public string FullName { get; set; }

        [ValidateNever]
        public List<NewsArticle> Articles { get; set; } = new();
    }
}
