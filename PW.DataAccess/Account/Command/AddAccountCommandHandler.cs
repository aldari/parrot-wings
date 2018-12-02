using PW.Core;
using PW.Core.Account.Command;
using PW.Core.Account.Domain;
using PW.Core.Cqs;
using PW.DataAccess.ApplicationData;
using PW.DataAccess.Cqs;
using System;
using System.Threading.Tasks;

namespace PW.DataAccess.Account.Command
{
    public class AddAccountCommandHandler : EfCommandHandlerBase<AddAccountCommand, ApplicationDbContext>
    {
        private readonly IClock _clock;

        public AddAccountCommandHandler(ApplicationDbContext dbContext, IClock clock)
            : base(dbContext)
        {
            _clock = clock;
        }

        public override async Task<CommandResult> ExecuteAsync(AddAccountCommand command)
        {
            var accountId = Guid.NewGuid();
            await DbContext.Accounts.AddAsync(new Core.Account.Domain.Account
            {
                Id = accountId,
                Name = command.Name,
                UserId = command.UserId
            });
            await DbContext.AccountTransactions.AddAsync(new AccountTransaction
            {
                CreditAccountId = AccountConst.SystemAccountGuid,
                DebitAccountId = accountId,
                Amount = 500,
                TransactionDate = _clock.Now
            });
            DbContext.SaveChanges();
            return new CommandResult();
        }
    }
}
