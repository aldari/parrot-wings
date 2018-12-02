using PW.Core.Account.Dto;
using System;
using System.Threading.Tasks;

namespace PW.Core.Account.Query
{
    public interface IGetFilteredAccountTransactionsQuery
    {
        Task<FilteredTransactionListDto> Execute(string sortColumn, string sortOrder, Guid accountId, int pageIndex, int pageSize, int? amount, DateTime? from, DateTime? to, Guid correspondent);
    }
}
