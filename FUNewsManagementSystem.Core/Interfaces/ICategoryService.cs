using FUNewsManagementSystem.Core.Models;

namespace FUNewsManagementSystem.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task<List<Category>> SearchAsync(string name, int? status);
        Task<bool> CanDeleteAsync(int id);
    }
}
