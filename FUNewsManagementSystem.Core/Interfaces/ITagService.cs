using FUNewsManagementSystem.Core.Models;


namespace FUNewsManagementSystem.Core.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(int id);
        Task AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(int id);
        Task<List<Tag>> SearchAsync(string name);
        Task<bool> CanDeleteAsync(int id);
    }
}
