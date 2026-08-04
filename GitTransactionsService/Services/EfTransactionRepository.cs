using Microsoft.EntityFrameworkCore;
using GitTransactionsService.Interfaces;
using GitTransactionsService.Data; 
using GitTransactionsService.Models;

namespace ProjeninAdi.Repositories // Kendi projenin adına göre düzenle
{
    public class EfTransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public EfTransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
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

        public void AddTransaction(string id, string message)
        {
            throw new NotImplementedException();
        }

        public string GetTransactionById(string id)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllTransactions()
        {
            throw new NotImplementedException();
        }
    }
}