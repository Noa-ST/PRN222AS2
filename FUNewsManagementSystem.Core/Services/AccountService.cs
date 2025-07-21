using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Account> AuthenticateAsync(string email, string password)
    {
        var account = await _accountRepository.GetByEmailAsync(email);
        if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
            return null;
        return account;
    }

    public async Task<Account> GetByIdAsync(int id)
    {
        return await _accountRepository.GetByIdAsync(id);
    }

    public async Task<List<Account>> GetAllAsync()
    {
        return await _accountRepository.GetAllAsync();
    }

    public async Task AddAsync(Account account)
    {
        account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
        await _accountRepository.AddAsync(account);
    }

    public async Task UpdateAsync(Account account)
    {
        if (!string.IsNullOrEmpty(account.Password))
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
        await _accountRepository.UpdateAsync(account);
    }

    public async Task DeleteAsync(int id)
    {
        await _accountRepository.DeleteAsync(id);
    }

    public async Task<List<Account>> SearchAsync(string email, int? role)
    {
        return await _accountRepository.SearchAsync(email, role);
    }

    public async Task<Account> GetByEmailAsync(string email)
    {
        return await _accountRepository.GetByEmailAsync(email); 
    }
}