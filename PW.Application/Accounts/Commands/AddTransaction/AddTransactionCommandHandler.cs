using MediatR;
using PW.Domain.Entities;
using PW.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PW.Application.Accounts.Commands.AddTransaction
{
    public class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand, CommandResult>
    {
        private readonly ApplicationDbContext _context;

        public AddTransactionCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommandResult> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.AccountTransactions.AddAsync(new AccountTransaction
                    {
                        Amount = request.Amount,
                        CreditAccountId = request.CreditAccount,
                        DebitAccountId = request.DebitAccount,
                        TransactionDate = DateTime.UtcNow
                    });
                    _context.SaveChanges();

                    int creditAccountBalance =
                        _context.AccountTransactions
                            .Where(x => x.CreditAccountId == request.CreditAccount ||
                                        x.DebitAccountId == request.CreditAccount)
                            .Sum(y => (y.CreditAccountId == request.CreditAccount) ? -y.Amount : y.Amount);

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
