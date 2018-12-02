using PW.Core;
using PW.Core.Account.Command;
using PW.Core.Account.Domain;
using PW.Core.Cqs;
using PW.DataAccess.ApplicationData;
using PW.DataAccess.Cqs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PW.DataAccess.Account.Command
{
    public class AddTransactionCommandHandler : EfCommandHandlerBase<AddTransactionCommand, ApplicationDbContext>
    {
        private readonly IClock _clock;

        public AddTransactionCommandHandler(ApplicationDbContext dbContext, IClock clock)
            : base(dbContext)
        {
            _clock = clock;
        }

        public override async Task<CommandResult> ExecuteAsync(AddTransactionCommand command)
        {
            using (var dbTransaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    await DbContext.AccountTransactions.AddAsync(new AccountTransaction
                    {
                        Amount = command.Amount,
                        CreditAccountId = command.CreditAccount,
                        DebitAccountId = command.DebitAccount,
                        TransactionDate = _clock.Now
                    });
                    DbContext.SaveChanges();

                    int creditAccountBalance =
                        DbContext.AccountTransactions
                            .Where(x => x.CreditAccountId == command.CreditAccount ||
                                        x.DebitAccountId == command.CreditAccount)
                            .Sum(y => (y.CreditAccountId == command.CreditAccount) ? -y.Amount : y.Amount);

                    if (creditAccountBalance < 0)
                    {
                        dbTransaction.Rollback();
                        return new CommandResult
                        {
                            Success = false,
                            Message = "insufficient funds"
                        };
                    }
                    else
                    {
                        dbTransaction.Commit();
                        return new CommandResult
                        {
                            Success = true
                        };
                    }
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                    return new CommandResult
                    {
                        Success = false,
                        Message = "Adding transaction failed"
                    };
                }
            }            
        }
    }
}
