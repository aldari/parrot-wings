using MediatR;
using System;

namespace PW.Application.Accounts.Queries.GetAccountTransactions
{
    public class AccountTransactionsQuery : IRequest<FilteredTransactionListDto>
    {
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public Guid AccountId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int? Amount { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid Correspondent { get; set; }

        public AccountTransactionsQuery()
        {
            SortOrder = "asc";
            PageIndex = 0;
            PageSize = 3;
        }
    }
}
