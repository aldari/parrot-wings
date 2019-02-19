using MediatR;
using System;

namespace PW.Application.Accounts.Commands.AddTransaction
{
    public class AddTransactionCommand: IRequest<CommandResult>
    {
        public Guid CreditAccount { get; set; }
        public Guid DebitAccount { get; set; }
        public int Amount { get; set; }
    }
}
