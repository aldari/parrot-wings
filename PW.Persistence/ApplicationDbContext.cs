using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PW.Domain.Entities;
using System;

namespace PW.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().Property(c => c.Name).IsRequired().HasMaxLength(30);

            modelBuilder.Entity<AccountTransaction>()
                .HasOne(p => p.DebitAccount)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AccountTransaction>()
                .HasOne(p => p.CreditAccount)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder
                .Entity<AccountTransaction>()
                .Property(e => e.TransactionDate)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }
    }
}
