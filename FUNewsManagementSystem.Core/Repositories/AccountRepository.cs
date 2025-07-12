using FUNewsManagementSystem.Core.Data;
using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Core.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FUNewsManagementContext _context;

        public AccountRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Account>> SearchAsync(string email, int? role)
        {
            var query = _context.Accounts.AsQueryable();
            if (!string.IsNullOrEmpty(email))
                query = query.Where(a => a.Email.Contains(email));
            if (role.HasValue)
                query = query.Where(a => a.Role == role.Value);
            return await query.ToListAsync();
        }
    }
}
