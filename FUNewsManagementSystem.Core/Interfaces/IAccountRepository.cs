using FUNewsManagementSystem.Core.Models;

namespace FUNewsManagementSystem.Core.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetByEmailAsync(string email);
        Task<Account> GetByIdAsync(int id);
        Task<List<Account>> GetAllAsync();
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(int id);
        Task<List<Account>> SearchAsync(string email, int? role);
    }
}
