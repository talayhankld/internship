using Microsoft.EntityFrameworkCore;
using GitTransactionsService.Models;

namespace GitTransactionsService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<Terminal> Terminals { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}