using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GitTransactionsService.Interfaces;
using GitTransactionsService.Data; 
using GitTransactionsService.Models;

namespace GitTransactionsService.Repositories 
{
    public class EfTransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public EfTransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // MERCHANT METHODS
        // ==========================================
        public async Task<bool> CheckIfNameExistsAsync(string name)
        {
            return await _context.Merchants.AnyAsync(x => x.Name == name);
        }

        public async Task<bool> CheckIfMerchantExistsAsync(string merchantId)
        {
            return await _context.Merchants.AnyAsync(m => m.MerchantId == merchantId);
        }

        public async Task<Merchant?> GetMerchantByIdAsync(string merchantId)
        {
            return await _context.Merchants.FirstOrDefaultAsync(m => m.MerchantId == merchantId);
        }

        public async Task CreateMerchantAsync(Merchant merchant)
        {
            await _context.Merchants.AddAsync(merchant);
            await _context.SaveChangesAsync();      
        }

        public async Task<IEnumerable<Merchant>> GetAllMerchantsAsync()
        {
            return await _context.Merchants.ToListAsync();
        }

        public async Task<bool> CheckMerchantStatusValidAsync(MerchantStatus status)
        {
            return await Task.FromResult(status == MerchantStatus.Active);
        }

        // ==========================================
        // TERMINAL METHODS
        // ==========================================
        public async Task<bool> CheckIfTerminalExistsAsync(string merchantId, string terminalNo)
        {
            return await _context.Terminals.AnyAsync(t => t.MerchantId == merchantId && t.TerminalNo == terminalNo);
        }

        public async Task<Terminal?> GetTerminalByNoAsync(string terminalNo)
        {
            return await _context.Terminals.FirstOrDefaultAsync(t => t.TerminalNo == terminalNo);
        }

        public async Task<Terminal?> GetTerminalAsync(string merchantId, string terminalNo)
        {
            return await _context.Terminals.FirstOrDefaultAsync(t => 
                t.MerchantId == merchantId && 
                t.TerminalNo == terminalNo);
        }

        public async Task AddTerminalAsync(Terminal terminal)
        {
            await _context.Terminals.AddAsync(terminal);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetTerminalsByMerchantIdAsync(string merchantId)
        {
            return await _context.Terminals
                .Where(t => t.MerchantId == merchantId)
                .Select(t => t.TerminalNo) 
                .ToListAsync();
        }
        public async Task<bool> IsTerminalValidAsync(string terminalNo) 
        {
            return await _context.Terminals.AnyAsync(t => t.TerminalNo == terminalNo);
        }

        public async Task<bool> CheckTerminalStatusValidAsync(TerminalStatus status)
        {
            return await Task.FromResult(status == TerminalStatus.Active);
        }

        // ==========================================
        // STATUS ENFORCER
        // ==========================================
        public async Task CheckStatusAsync(TerminalStatus status, MerchantStatus merchantStatus)
        {
            if (merchantStatus == MerchantStatus.Inactive) 
            {
                throw new InvalidOperationException("Merchant is deactivated, terminals are not useable.");
            }

            if (status == TerminalStatus.Inactive) 
            {
                throw new InvalidOperationException("The terminal that you try to transact is deactivated.");
            }
        }

        public async Task<bool> IsMerchantActiveAsync(string merchantId)
        {   
            var merchant = await _context.Merchants.FirstOrDefaultAsync(m => m.MerchantId == merchantId);
            return merchant != null && merchant.Status == MerchantStatus.Active;
        }

        public async Task<bool> IsTerminalActiveAsync(string merchantId, string terminalNo)
        {
            var terminal = await _context.Terminals.FirstOrDefaultAsync(t => 
                    t.MerchantId == merchantId && 
                    t.TerminalNo == terminalNo);            
                    return terminal != null && terminal.Status == TerminalStatus.Active;        
        }

        // ==========================================
        // TRANSACTION METHODS
        // ==========================================
        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync(); 
        }

        public async Task<Transaction?> GetByRefNumberAsync(string referenceNumber)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.ReferenceNumber == referenceNumber);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }
    }
}