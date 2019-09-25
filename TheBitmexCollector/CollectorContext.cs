using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace TheBitmexCollector
{
    public class CollectorContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;port=3306;user=user;database=collector;");
        }

        public DbSet<Liquidation> Liquidations { get; set; }
        public DbSet<TradeBin> TradeBins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Liquidation>()
                .HasIndex(b => b.Symbol);
            modelBuilder.Entity<Liquidation>()
                .HasKey(b => b.LiquidationId);
            modelBuilder.Entity<TradeBin>()
                .HasIndex(b => b.Symbol);
            modelBuilder.Entity<TradeBin>()
                .HasIndex(b => b.Timestamp);
        }
    }
}
