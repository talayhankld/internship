using GitTransactionsService.Models;

namespace GitTransactionsService.Interfaces
{
    public interface ITransactionRepository
    {
        Task<bool> CheckIfNameExistsAsync(string name);
        Task CreateMerchantAsync(Merchant merchant);
        Task<bool> CheckIfTerminalExistsAsync(string merchantId, string terminalNo);
        Task<bool> CheckIfMerchantExistsAsync(string merchantId);

        Task CreateTerminalAsync(Terminal terminal);
        Task AddTransactionAsync(Transaction transaction);
        Task CancelTransactionAsync(string refNumber);
        Task<Transaction?> GetByRefNumberAsync(string refNumber);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task AddTerminalAsync(Terminal terminal);
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction); 
        Task<bool> IsTerminalValidAsync(string terminalId);
    }
}
