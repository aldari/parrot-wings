//using Microsoft.EntityFrameworkCore;
//using NUnit.Framework;
//using PW.Domain.Entities;
//using PW.Persistence;
//using System;
//using System.Threading.Tasks;

//namespace PW.Application.Tests.Integration
//{
//    [TestFixture]
//    public class GetLastAccountTransactionsQueryTests
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly string _databaseName;

//        public GetLastAccountTransactionsQueryTests()
//        {
//            _databaseName = Guid.NewGuid().ToString();

//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                      .UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=PWTest{_databaseName};Trusted_Connection=True;MultipleActiveResultSets=true")
//                      .Options;
//            _context = new ApplicationDbContext(options);
//            _context.Database.Migrate();
//            PWDatabaseInitializer.Initialize(_context);
//        }

//        [Test]
//        public async Task ReturnsLastAccountTransactionAsync()
//        {
//            Guid SystemAccountGuid = Guid.Parse("dd913452-8031-4063-97cd-2ff3e65a922c");
//            _context.Accounts.Add(new Account
//            {
//                Id = SystemAccountGuid,
//                Name = "System"
//            });
//            Guid PetrAccountGuid = Guid.Parse("1b4d5e36-80f5-430c-8c05-f40048b4e54f");
//            _context.Accounts.Add(new Account
//            {
//                Id = PetrAccountGuid,
//                Name = "Petr"
//            });
//            Guid NikolayAccountGuid = Guid.Parse("e7b207e0-61a5-4a59-8a28-fdb69cfc881f");
//            _context.Accounts.Add(new Account
//            {
//                Id = NikolayAccountGuid,
//                Name = "Nikolay"
//            });

//            _context.AccountTransactions.AddRange(
//            new AccountTransaction
//            {
//                Amount = 500,
//                CreditAccountId = SystemAccountGuid,
//                DebitAccountId = PetrAccountGuid,
//                TransactionDate = DateTime.Now
//            },
//            new AccountTransaction
//            {
//                Amount = 500,
//                CreditAccountId = SystemAccountGuid,
//                DebitAccountId = NikolayAccountGuid,
//                TransactionDate = DateTime.Now
//            },
//            new AccountTransaction
//            {
//                Amount = 50,
//                CreditAccountId = NikolayAccountGuid,
//                DebitAccountId = PetrAccountGuid,
//                TransactionDate = DateTime.Now
//            },
//            new AccountTransaction
//            {
//                Amount = 60,
//                CreditAccountId = PetrAccountGuid,
//                DebitAccountId = NikolayAccountGuid,
//                TransactionDate = DateTime.Now
//            },
//            new AccountTransaction
//            {
//                Amount = 70,
//                CreditAccountId = NikolayAccountGuid,
//                DebitAccountId = PetrAccountGuid,
//                TransactionDate = DateTime.Now
//            },
//            new AccountTransaction
//            {
//                Amount = 80,
//                CreditAccountId = PetrAccountGuid,
//                DebitAccountId = NikolayAccountGuid,
//                TransactionDate = DateTime.Now
//            });
//            await _context.SaveChangesAsync();

//            var query = new GetLastAccountTransactionsQuery(_context);

//            await query.Execute(PetrAccountGuid);
//            var balance = await query.Execute(PetrAccountGuid);

//            Assert.AreEqual(5, balance.Count);
//        }
//    }
//}
