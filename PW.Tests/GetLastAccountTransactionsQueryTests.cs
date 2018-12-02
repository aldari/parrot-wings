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
    public class GetLastAccountTransactionsQueryTests
    {
        private readonly ApplicationDbContext _context;

        public GetLastAccountTransactionsQueryTests()
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
        public async Task ReturnsLastAccountTransactionAsync()
        {
            Guid SystemAccountGuid = Guid.Parse("dd913452-8031-4063-97cd-2ff3e65a922c");
            _context.Accounts.Add(new Account
            {
                Id = SystemAccountGuid,
                Name = "System"
            });
            Guid PetrAccountGuid = Guid.Parse("1b4d5e36-80f5-430c-8c05-f40048b4e54f");
            _context.Accounts.Add(new Account
            {
                Id = PetrAccountGuid,
                Name = "Petr"
            });
            Guid NikolayAccountGuid = Guid.Parse("e7b207e0-61a5-4a59-8a28-fdb69cfc881f");
            _context.Accounts.Add(new Account
            {
                Id = NikolayAccountGuid,
                Name = "Nikolay"
            });

            _context.AccountTransactions.AddRange(
            new AccountTransaction
            {
                Amount = 500,
                CreditAccountId = SystemAccountGuid,
                DebitAccountId = PetrAccountGuid,
                TransactionDate = DateTime.Now
            },
            new AccountTransaction
            {
                Amount = 500,
                CreditAccountId = SystemAccountGuid,
                DebitAccountId = NikolayAccountGuid,
                TransactionDate = DateTime.Now
            },
            new AccountTransaction
            {
                Amount = 50,
                CreditAccountId = NikolayAccountGuid,
                DebitAccountId = PetrAccountGuid, TransactionDate = DateTime.Now
            },
            new AccountTransaction
            {
                Amount = 60,
                CreditAccountId = PetrAccountGuid,
                DebitAccountId = NikolayAccountGuid,
                TransactionDate = DateTime.Now
            },
            new AccountTransaction
            {
                Amount = 70,
                CreditAccountId = NikolayAccountGuid,
                DebitAccountId = PetrAccountGuid,
                TransactionDate = DateTime.Now
            },
            new AccountTransaction
            {
                Amount = 80,
                CreditAccountId = PetrAccountGuid,
                DebitAccountId = NikolayAccountGuid,
                TransactionDate = DateTime.Now
            });
            await _context.SaveChangesAsync();

            var query = new GetLastAccountTransactionsQuery(_context);

            await query.Execute(PetrAccountGuid);
            var balance = await query.Execute(PetrAccountGuid);

            Assert.AreEqual(5, balance.Count);
        }
    }
}
