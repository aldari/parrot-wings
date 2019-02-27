using Microsoft.EntityFrameworkCore;
using NFluent;
using NUnit.Framework;
using PW.Application.Accounts.Commands.AddAccount;
using PW.Application.Accounts.Commands.AddTransaction;
using PW.Application.Accounts.Queries.GetAccountBalance;
using PW.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PW.Application.Tests.Integration
{
    [TestFixture]
    public class AccountBalanceQueryHandlerTest : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly string _databaseName;

        public AccountBalanceQueryHandlerTest()
        {
            _databaseName = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                      .UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=PWTest{_databaseName};Trusted_Connection=True;MultipleActiveResultSets=true")
                      .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.Migrate();
            PWDatabaseInitializer.Initialize(_context);
        }

        [Test]
        public async Task Handle_Returns500_ForInitialBalance()
        {
            // Arrange
            var ivanAccountCommand = new AddAccountCommand { UserId = Guid.NewGuid(), Name = "Ivan" };
            var ivanAccountId = await new AddAccountCommandHandler(_context).Handle(ivanAccountCommand, CancellationToken.None);

            // Act
            var handler = new AccountBalanceQueryHandler(_context);
            var balance = await handler.Handle(new AccountBalanceQuery {AccountId = ivanAccountId }, CancellationToken.None);

            // Assert
            Check.That(500).Equals(balance.Balance);
        }

        [Test]
        public async Task Handle_BalanceIncreasing_AfterSendingToThisAccount()
        {
            // Arrange
            var ivanAccountCommand = new AddAccountCommand { UserId = Guid.NewGuid(), Name = "Ivan" };
            var ivanAccountId = await new AddAccountCommandHandler(_context).Handle(ivanAccountCommand, CancellationToken.None);

            var petrAccountCommand = new AddAccountCommand { UserId = Guid.NewGuid(), Name = "Petr" };
            var petrAccountId = await new AddAccountCommandHandler(_context).Handle(petrAccountCommand, CancellationToken.None);

            var transaction = new AddTransactionCommand { Amount = 11, CreditAccount = petrAccountId, DebitAccount = ivanAccountId };
            await new AddTransactionCommandHandler(_context).Handle(transaction, CancellationToken.None);

            // Act
            var handler = new AccountBalanceQueryHandler(_context);
            var balance = await handler.Handle(new AccountBalanceQuery { AccountId = ivanAccountId }, CancellationToken.None);

            // Assert
            Check.That(511).Equals(balance.Balance);
        }

        [Test]
        public async Task Handle_BalanceDecreasing_AfterSendingFromThisAccount()
        {
            // Arrange
            var ivanAccountCommand = new AddAccountCommand { UserId = Guid.NewGuid(), Name = "Ivan" };
            var ivanAccountId = await new AddAccountCommandHandler(_context).Handle(ivanAccountCommand, CancellationToken.None);

            var petrAccountCommand = new AddAccountCommand { UserId = Guid.NewGuid(), Name = "Petr" };
            var petrAccountId = await new AddAccountCommandHandler(_context).Handle(petrAccountCommand, CancellationToken.None);

            var transaction = new AddTransactionCommand { Amount = 13, CreditAccount = ivanAccountId, DebitAccount = petrAccountId };
            await new AddTransactionCommandHandler(_context).Handle(transaction, CancellationToken.None);

            // Act
            var handler = new AccountBalanceQueryHandler(_context);
            var balance = await handler.Handle(new AccountBalanceQuery { AccountId = ivanAccountId }, CancellationToken.None);

            // Assert
            Check.That(487).Equals(balance.Balance);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
