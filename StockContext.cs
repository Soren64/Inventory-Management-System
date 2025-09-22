using Microsoft.EntityFrameworkCore;
using StockManager.Models;

namespace StockManager.Data
{
    public class StockContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public StockContext(DbContextOptions<StockContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Local SQLite database
            optionsBuilder.UseSqlite("Data Source=stockmanager.db");
        }
    }
}