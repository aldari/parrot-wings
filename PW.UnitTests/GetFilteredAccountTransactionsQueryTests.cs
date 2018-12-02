using Microsoft.EntityFrameworkCore;
using NFluent;
using NUnit.Framework;
using PW.Core.Account.Domain;
using PW.DataAccess.Account.Query;
using PW.DataAccess.ApplicationData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PW.UnitTests
{
    [TestFixture]
    public class GetFilteredAccountTransactionsQueryTests
    {
        private List<AccountTransaction> UsersQuery { get; }
        private ApplicationDbContext Context { get; set; }

        private static readonly Guid SystemAccount = Guid.NewGuid();
        private static readonly Guid JohnAccount = Guid.NewGuid();
        private static readonly Guid SherlockAccount = Guid.NewGuid();
        private static readonly Guid MaryAccount = Guid.NewGuid();

        public GetFilteredAccountTransactionsQueryTests()
        {
            UsersQuery = new List<AccountTransaction>
            {
                new AccountTransaction
                {
                    Id = 1,
                    Amount = 500,
                    CreditAccountId = SystemAccount,
                    DebitAccountId = JohnAccount,
                    TransactionDate = new DateTime(2018, 01, 01)
                },
                new AccountTransaction
                {
                    Id = 2,
                    Amount = 500,
                    CreditAccountId = SystemAccount,
                    DebitAccountId = MaryAccount,
                    TransactionDate = new DateTime(2018, 01, 02)
                },
                new AccountTransaction
                {
                    Id = 3,
                    Amount = 30,
                    CreditAccountId = JohnAccount,
                    DebitAccountId = MaryAccount,
                    TransactionDate = new DateTime(2018, 01, 03)
                },
                new AccountTransaction
                {
                    Id = 4,
                    Amount = 31,
                    CreditAccountId = MaryAccount,
                    DebitAccountId = JohnAccount,
                    TransactionDate = new DateTime(2018, 01, 04)
                },
                new AccountTransaction
                {
                    Id = 5,
                    Amount = 32,
                    CreditAccountId = JohnAccount,
                    DebitAccountId = SherlockAccount,
                    TransactionDate = new DateTime(2018, 01, 05)
                }
            };
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Test")
                .Options;

            Context = new ApplicationDbContext(options);

            UsersQuery.ForEach(x =>
            {
                Context.Add(x);
                Context.SaveChanges();
            });
        }

        [Test]
        public async Task Test()
        {
            // Arrange
            var searchModel = new TransactionSearchViewModel
            {
                Amount = 31,
                ClientId = MaryAccount
            };
            var query = new GetFilteredAccountTransactionsQuery(Context);

            // Act
            var result = await query.Execute("", "", searchModel.ClientId, 0, 5, searchModel.Amount, searchModel.From,
                searchModel.To, searchModel.Correspondent);

            // Assert
            Check.That(result.Transactions.Extracting("Amount")).ContainsExactly(31);
        }

        [Test]
        public async Task Test2()
        {
            // Arrange
            var searchModel = new TransactionSearchViewModel
            {
                From = new DateTime(2018, 1, 3),
                To = new DateTime(2018, 1, 4),
                ClientId = MaryAccount
            };
            var query = new GetFilteredAccountTransactionsQuery(Context);

            // Act
            var result = await query.Execute("", "", searchModel.ClientId, 0, 5, searchModel.Amount, searchModel.From,
                searchModel.To, searchModel.Correspondent);

            // Assert
            Check.That(result.Transactions.Extracting("Amount")).ContainsExactly(30, 31);
        }

        [Test]
        public async Task Test3()
        {
            // Arrange
            var searchModel = new TransactionSearchViewModel
            {
                ClientId = JohnAccount
            };
            var query = new GetFilteredAccountTransactionsQuery(Context);

            // Act
            var result = await query.Execute("", "", searchModel.ClientId, 0, 5, searchModel.Amount, searchModel.From,
                searchModel.To, searchModel.Correspondent);

            // Assert
            Check.That(result.Transactions.Extracting("Id")).ContainsExactly(1, 3, 4, 5);
        }

        [Test]
        public async Task Test4()
        {
            // Arrange
            var searchModel = new TransactionSearchViewModel
            {
                ClientId = MaryAccount,
                Correspondent = JohnAccount 
            };
            var query = new GetFilteredAccountTransactionsQuery(Context);

            // Act
            var result = await query.Execute("", "", searchModel.ClientId, 0, 5, searchModel.Amount, searchModel.From,
                searchModel.To, searchModel.Correspondent);

            // Assert
            Check.That(result.Transactions.Extracting("Id")).ContainsExactly(3, 4);
        }
    }
}