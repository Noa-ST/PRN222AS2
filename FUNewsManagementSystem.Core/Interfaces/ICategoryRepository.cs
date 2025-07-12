using FUNewsManagementSystem.Core.Models;

namespace FUNewsManagementSystem.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task<List<Category>> SearchAsync(string name, int? status);
        Task<bool> HasArticlesAsync(int id);
    }
}
