using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NFluent;
using NUnit.Framework;
using PW.Application.Accounts;
using PW.Application.Accounts.Queries.GetAccountTransactions;
using PW.Domain.Entities;
using PW.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PW.Application.Tests.Unit
{
    [TestFixture]
    public class AccountTransactionsQueryHandlerTests
    {
        private List<AccountTransaction> UsersQuery { get; }
        private List<Account> Accounts { get; }
        private ApplicationDbContext Context { get; set; }

        private static readonly Guid SystemAccount = Guid.NewGuid();
        private static readonly Guid JohnAccount = Guid.NewGuid();
        private static readonly Guid SherlockAccount = Guid.NewGuid();
        private static readonly Guid MaryAccount = Guid.NewGuid();
        private readonly IMapper _mapper;

        public AccountTransactionsQueryHandlerTests()
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

            Accounts = new List<Account>
            {
                new Account {
                    Id = SystemAccount,
                    Name = "SystemAccount"
                },
                new Account {
                    Id = JohnAccount,
                    Name = "John"
                },
                new Account {
                    Id = SherlockAccount,
                    Name = "Sherlock"
                },
                new Account {
                    Id = MaryAccount,
                    Name = "Mary"
                },
            };

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new AccountMappingProfile()));
            _mapper = new Mapper(configuration);
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            Context = new ApplicationDbContext(options);

            Accounts.ForEach(x =>
            {
                Context.Add(x);
                Context.SaveChanges();
            });
            UsersQuery.ForEach(x =>
            {
                Context.Add(x);
                Context.SaveChanges();
            });
        }

        [Test]
        public async Task Handle_Returns1Item_WhenFilterByAmountEqual31()
        {
            // Arrange
            var query = new AccountTransactionsQuery { AccountId = MaryAccount, Amount = 31,  };
            var handler = new AccountTransactionsQueryHandler(Context, _mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Check.That(result.Transactions.Extracting("Amount")).ContainsExactly(31);
        }

        [Test]
        public async Task Handle_Returns2Items_WhenFilterByDate()
        {
            // Arrange
            var query = new AccountTransactionsQuery {
                AccountId = MaryAccount,
                From = new DateTime(2018, 1, 3),
                To = new DateTime(2018, 1, 4)
            };
            var handler = new AccountTransactionsQueryHandler(Context, _mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Check.That(result.Transactions.Extracting("Amount")).ContainsExactly(30, 31);
        }

        [Test]
        public async Task Handle_Returns4Items_WhenNoFiltersForJohnAccount()
        {
            // Arrange
            var query = new AccountTransactionsQuery
            {
                AccountId = JohnAccount,
                PageSize = 5
            };
            var handler = new AccountTransactionsQueryHandler(Context, _mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Check.That(result.Transactions.Extracting("Id")).ContainsExactly(1, 3, 4, 5);
        }

        [Test]
        public async Task Handle_Returns2Items_WhenFilterByCorrespondent()
        {
            // Arrange
            var query = new AccountTransactionsQuery
            {
                AccountId = MaryAccount,
                Correspondent = JohnAccount
            };
            var handler = new AccountTransactionsQueryHandler(Context, _mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Check.That(result.Transactions.Extracting("Id")).ContainsExactly(3, 4);
        }
    }
}