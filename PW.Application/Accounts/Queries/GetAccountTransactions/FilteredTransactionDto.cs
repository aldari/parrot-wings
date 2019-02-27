using System;

namespace PW.Application.Accounts.Queries.GetAccountTransactions
{
    public class FilteredTransactionDto
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Correspondent { get; set; }
        public Guid CorrespondentId { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsCredit { get; set; }
    }
}
