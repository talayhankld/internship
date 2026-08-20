using GitTransactionsService.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitTransactionsService.Interfaces
{
    public interface ITransactionRepository
    {
        // --- MERCHANT METHODS ---
        Task<bool> CheckIfNameExistsAsync(string name);
        Task<bool> CheckIfMerchantExistsAsync(string merchantId);
        Task<Merchant?> GetMerchantByIdAsync(string merchantId);
        Task CreateMerchantAsync(Merchant merchant);
        Task<bool> IsMerchantActiveAsync(string merchantId);

        Task<bool> CheckMerchantStatusValidAsync(MerchantStatus status);
        Task<IEnumerable<Merchant>> GetAllMerchantsAsync();

        // --- TERMINAL METHODS ---
        Task<bool> CheckIfTerminalExistsAsync(string merchantId, string terminalNo);
        Task<Terminal?> GetTerminalByNoAsync(string terminalNo);
        Task<Terminal?> GetTerminalAsync(string merchantId, string terminalNo);
        Task AddTerminalAsync(Terminal terminal);
        Task<bool> IsTerminalValidAsync(string terminalNo);
        Task<bool> IsTerminalActiveAsync(string merchantId, string terminalNo);
        Task<bool> CheckTerminalStatusValidAsync(TerminalStatus status);

        // --- STATUS ENFORCER ---
        Task CheckStatusAsync(TerminalStatus status, MerchantStatus merchantStatus);

        // --- TRANSACTION METHODS ---
        Task AddAsync(Transaction transaction);
        Task<IEnumerable<string>> GetTerminalsByMerchantIdAsync(string merchantId);
        Task<Transaction?> GetByRefNumberAsync(string referenceNumber);
        Task UpdateAsync(Transaction transaction); 
        Task<IEnumerable<Transaction>> GetAllAsync();
    }
}