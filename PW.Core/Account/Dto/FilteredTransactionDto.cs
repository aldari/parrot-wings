using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Core.Account.Dto
{
    public class FilteredTransactionDto
    {
        public int Amount { get; set; }
        public string Correspondent { get; set; }
        public Guid CorrespondentId { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsCredit { get; set; }
    }
}
