using System;
using PW.Core.Cqs;

namespace PW.Core.Account.Command
{
    public class AddTransactionCommand: ICommand
    {
        public Guid CreditAccount { get; }
        public Guid DebitAccount { get; }
        public int Amount { get; }


        public AddTransactionCommand(Guid creditAccount, Guid debitAccount, int amount)
        {
            CreditAccount = creditAccount;
            DebitAccount = debitAccount;
            Amount = amount;
        }
    }
}
