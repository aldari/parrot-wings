using System.Collections.Generic;

namespace PW.Application.Accounts.Queries.GetAccountTransactions
{
    public class FilteredTransactionListDto
    {
        public List<FilteredTransactionDto> Transactions { get; set; }
        public int Count { get; set; }
    }
}
