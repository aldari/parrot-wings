using MediatR;
using PW.Domain.Entities;
using PW.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PW.Application.Accounts.Commands.AddAccount
{
    public class AddAccountCommandHandler : IRequestHandler<AddAccountCommand, Guid>
    {
        private readonly ApplicationDbContext _context;

        public AddAccountCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(AddAccountCommand request, CancellationToken cancellationToken)
        {
            var accountId = Guid.NewGuid();
            var account = new Account
            {
                Id = accountId,
                Name = request.Name,
                UserId = request.UserId
            };
            await _context.Accounts.AddAsync(account);
            await _context.AccountTransactions.AddAsync(new AccountTransaction
            {
                CreditAccountId = AccountConst.SystemAccountGuid,
                DebitAccountId = accountId,
                Amount = 500,
                TransactionDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync(cancellationToken);
            return account.Id;
        }
    }
}
