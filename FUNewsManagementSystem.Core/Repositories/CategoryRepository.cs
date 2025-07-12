using FUNewsManagementSystem.Core.Data;
using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FUNewsManagementContext _context;

        public CategoryRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Category>> SearchAsync(string name, int? status)
        {
            var query = _context.Categories.AsQueryable();

            // Chỉ áp dụng bộ lọc tên nếu name không rỗng
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.CategoryName.Contains(name));
            }

            // Áp dụng bộ lọc trạng thái nếu status có giá trị
            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> HasArticlesAsync(int id)
        {
            return await _context.NewsArticles.AnyAsync(a => a.CategoryId == id);
        }
    }
}
