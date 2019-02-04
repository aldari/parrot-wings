using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Application.Accounts.Queries.GetAccountTransactions
{
    public class TransactionSearchViewModel
    {
        public int? Amount { get; set; }
        public Guid ClientId { get; set; }
        public Guid Correspondent { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
