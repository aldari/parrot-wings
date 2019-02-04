using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Domain.Entities
{
    public class AccountTransaction
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Amount { get; set; }
        public Guid DebitAccountId { get; set; }
        public Guid CreditAccountId { get; set; }
        public virtual Account DebitAccount { get; set; }
        public virtual Account CreditAccount { get; set; }
    }
}
