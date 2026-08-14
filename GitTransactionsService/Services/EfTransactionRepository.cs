using Microsoft.EntityFrameworkCore;
using GitTransactionsService.Interfaces;
using GitTransactionsService.Data; 
using GitTransactionsService.Models;

namespace GitTransactionsService.Repositories 
{
    public class EfTransactionRepository : ITransactionRepository
    {
        public async Task<bool> CheckIfNameExistsAsync(string name)
        {
        return await _context.Merchants.AnyAsync(x => x.Name == name);
        }
        
        private readonly AppDbContext _context;

        public EfTransactionRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task CreateMerchantAsync(Merchant merchant)
        {
            await _context.Merchants.AddAsync(merchant);
            await _context.SaveChangesAsync();      
        }

        public async Task<bool> CheckIfTerminalExistsAsync(string merchantId, string terminalNo)
        {
            return await _context.Terminals.AnyAsync(t => 
            t.MerchantId == merchantId && 
            t.TerminalNo == terminalNo);
        }
        public async Task<bool> CheckIfMerchantExistsAsync(string merchantId)
        {
            return await _context.Merchants.AnyAsync(m => m.MerchantId == merchantId);
        }

        public async Task CreateTerminalAsync(Terminal terminal)
        {
            await _context.Terminals.AddAsync(terminal);
            await _context.SaveChangesAsync();      
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();      
        }
        
        public async Task CancelTransactionAsync(string ReferenceNumber)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.ReferenceNumber == ReferenceNumber);
            if (transaction != null)
            {
                transaction.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Transaction?> GetByRefNumberAsync(string ReferenceNumber)
        {
        return await _context.Transactions.FirstOrDefaultAsync(t => t.ReferenceNumber == ReferenceNumber);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions.ToListAsync();
        }
        
        public async Task AddTerminalAsync(Terminal terminal)
        {
            await _context.Terminals.AddAsync(terminal);
            await _context.SaveChangesAsync();
        }
        
        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync(); 
        }
        
        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTerminalValidAsync(string terminalNo) 
        {
    
            return await _context.Terminals.AnyAsync(t => t.TerminalNo == terminalNo);
        }

    }
}