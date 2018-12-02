using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PW.Core.Account.Domain;
using PW.DataAccess.Account.Query;
using PW.DataAccess.ApplicationData;
using System;
using System.Threading.Tasks;

namespace PW.Tests
{
    [TestFixture]
    public class GetAccountBalanceQueryTests
    {
        private readonly ApplicationDbContext _context;

        public GetAccountBalanceQueryTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=testPW;Trusted_Connection=True;MultipleActiveResultSets=true")
                .UseInternalServiceProvider(serviceProvider);

            _context = new ApplicationDbContext(builder.Options);
            _context.Database.Migrate();
        }

        [SetUp]
        public async Task ClearTablesAsync()
        {
            _context.AccountTransactions.RemoveRange(_context.AccountTransactions);
            _context.Accounts.RemoveRange(_context.Accounts);
            await _context.SaveChangesAsync();
        }

        [Test]
        public async Task GetAccountBalanceQueryReturnsInitialBalanceEqual500()
        {
            _context.Accounts.Add(new Account
            {
                Id = Guid.Parse("dd913452-8031-4063-97cd-2ff3e65a922c"),
                Name = "System"
            });
            _context.Accounts.Add(new Account
            {
                Id = Guid.Parse("1b4d5e36-80f5-430c-8c05-f40048b4e54f"),
                Name = "Petr"
            });
            _context.AccountTransactions.Add(new AccountTransaction { Amount = 500, CreditAccountId = Guid.Parse("dd913452-8031-4063-97cd-2ff3e65a922c"), DebitAccountId = Guid.Parse("1b4d5e36-80f5-430c-8c05-f40048b4e54f"), TransactionDate = DateTime.Now });
            await _context.SaveChangesAsync();
            var query = new GetAccountBalanceQuery(_context);

            var balance = await query.Execute(Guid.Parse("1b4d5e36-80f5-430c-8c05-f40048b4e54f"));

            Assert.AreEqual(500, balance);
        }

        [Test]
        public async Task GetAccountBalanceQueryReturnsBalanceEqual320AfterTransaction()
        {
            _context.Accounts.Add(new Account
            {
                Id = Guid.Parse("dd913452-8031-4063-97cd-2ff3e65a922c"),
                Name = "System"
            });
            _context.Accounts.Add(new Account
            {
                Id = Guid.Parse("1b4d5e36-80f5-430c-8c05-f40048b4e54f"),
                Name = "Petr"
            });
            _context.AccountTransactions.Add(new AccountTransaction { Amount = 500, CreditAccountId = Guid.Parse("dd913452-8031-4063-97cd-2ff3e65a922c"), DebitAccountId = Guid.Parse("1b4d5e36-80f5-430c-8c05-f40048b4e54f"), TransactionDate = DateTime.Now });
            await _context.SaveChangesAsync();
            var query = new GetAccountBalanceQuery(_context);

            var balance = await query.Execute(Guid.Parse("1b4d5e36-80f5-430c-8c05-f40048b4e54f"));

            Assert.AreEqual(500, balance);
        }
    }
}
