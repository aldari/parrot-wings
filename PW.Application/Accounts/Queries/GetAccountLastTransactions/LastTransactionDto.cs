using System;

namespace PW.Application.Accounts.Queries.GetAccountLastTransactions
{
    public class LastTransactionDto
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public Guid AccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AccumulateSum { get; set; }
        public string AccountName { get; set; }
    }
}
