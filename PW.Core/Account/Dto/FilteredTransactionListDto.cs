using PW.Core.Account.Domain;
using System.Collections.Generic;

namespace PW.Core.Account.Dto
{
    public class FilteredTransactionListDto
    {
        public List<FilteredTransactionDto> Transactions { get; set; }
        public int Count { get; set; }
    }
}
