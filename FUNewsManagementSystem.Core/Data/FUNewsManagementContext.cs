using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Core.Models;

namespace FUNewsManagementSystem.Core.Data
{
    public class FUNewsManagementContext : DbContext
    {
        public FUNewsManagementContext(DbContextOptions<FUNewsManagementContext> options)
     : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsArticleTag> NewsArticleTags { get; set; }

 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình khóa chính cho NewsArticleTag
            modelBuilder.Entity<NewsArticleTag>()
                .HasKey(nat => new { nat.ArticleId, nat.TagId });

            // Cấu hình mối quan hệ nhiều-nhiều
            modelBuilder.Entity<NewsArticleTag>()
                .HasOne(nat => nat.Article)
                .WithMany(a => a.NewsArticleTags)
                .HasForeignKey(nat => nat.ArticleId);

            modelBuilder.Entity<NewsArticleTag>()
                .HasOne(nat => nat.Tag)
                .WithMany(t => t.NewsArticleTags)
                .HasForeignKey(nat => nat.TagId);

            // Dữ liệu khởi tạo cho tài khoản admin
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = 1,
                    Email = "admin@FUNewsManagementSystem.org",
                    Password = BCrypt.Net.BCrypt.HashPassword("@@abc123@@"),
                    Role = 0, // Admin
                    FullName = "Administrator"
                }
            );

            // Dữ liệu khởi tạo mẫu cho Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Tin tức chung", Status = 1 },
                new Category { CategoryId = 2, CategoryName = "Sự kiện", Status = 1 }
            );

            // Dữ liệu khởi tạo mẫu cho Tags
            modelBuilder.Entity<Tag>().HasData(
                new Tag { TagId = 1, TagName = "Giáo dục" },
                new Tag { TagId = 2, TagName = "Công nghệ" }
            );
        }
    }
}