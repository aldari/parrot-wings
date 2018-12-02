using PW.Core.Account.Domain;
using PW.Core.Account.Dto;
using PW.Core.Account.Query;
using PW.DataAccess.ApplicationData;
using PW.DataAccess.Cqs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.DataAccess.Account.Query
{
    public class GetFilteredAccountTransactionsQuery : EfQueryBase<ApplicationDbContext>, IGetFilteredAccountTransactionsQuery
    {
        public GetFilteredAccountTransactionsQuery(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<FilteredTransactionListDto> Execute(string sortColumn, string sortOrder, Guid accountId, int pageIndex, int pageSize, int? amount, DateTime? fromTransactionDate, DateTime? toTransactionDate, Guid correspondent)
        {
            var searchFieldMutators = new List<SearchFieldMutator<AccountTransaction, TransactionSearchViewModel>>
            {
                // Default filter by ClientId without correspondent
                new SearchFieldMutator<AccountTransaction, TransactionSearchViewModel>(
                    search => search.Correspondent == Guid.Empty,
                    (transactions, search) =>
                        transactions.Where(u => u.DebitAccountId == search.ClientId || u.CreditAccountId == search.ClientId)),

                // Correspondent and ClientId filter
                new SearchFieldMutator<AccountTransaction, TransactionSearchViewModel>(
                    search => search.Correspondent != Guid.Empty,
                    (transactions, search) =>
                        transactions.Where(u => (u.DebitAccountId == search.ClientId
                        &&  u.CreditAccountId == search.Correspondent )
                        || (u.DebitAccountId == search.Correspondent
                            &&  u.CreditAccountId == search.ClientId ))),

                // Amount Filter
                new SearchFieldMutator<AccountTransaction, TransactionSearchViewModel>(
                    search => search.Amount.HasValue,
                    (transactions, search) =>
                        transactions.Where(u => u.Amount == search.Amount)),

                // Date From Filter
                new SearchFieldMutator<AccountTransaction, TransactionSearchViewModel>(
                    search => search.From.HasValue,
                    (transactions, search) =>
                        transactions.Where(u => search.From.HasValue && u.TransactionDate >= search.From)),

                // Date To Filter
                new SearchFieldMutator<AccountTransaction, TransactionSearchViewModel>(
                    search => search.To.HasValue,
                    (transactions, search) =>
                        transactions.Where(u => search.To.HasValue && u.TransactionDate <= search.To)),

                // sort order
                // sort by transaction date
                new SearchFieldMutator<AccountTransaction, TransactionSearchViewModel>(
                    search => search.SortColumn == "transactionDate" && search.SortOrder == "asc",
                    (transactions, search) => transactions.OrderBy(u => u.TransactionDate).ThenBy(u => u.Id)),
                new SearchFieldMutator<AccountTransaction, TransactionSearchViewModel>(
                    search => search.SortColumn == "transactionDate" && search.SortOrder == "desc",
                    (transactions, search) => transactions.OrderByDescending(u => u.TransactionDate).ThenBy(u => u.Id)),
                // sort by amount
                new SearchFieldMutator<AccountTransaction, TransactionSearchViewModel>(
                    search => search.SortColumn == "amount" && search.SortOrder == "asc",
                    (transactions, search) => transactions.OrderBy(u => u.Amount).ThenBy(u => u.Id)),
                new SearchFieldMutator<AccountTransaction, TransactionSearchViewModel>(
                    search => search.SortColumn == "amount" && search.SortOrder == "desc",
                    (transactions, search) => transactions.OrderByDescending(u => u.Amount).ThenBy(u => u.Id)),
            };

            var searchFieldMutators2 = new List<SearchFieldMutator<FilteredTransactionDto, TransactionSearchViewModel>>
            {
                // sort by correspondent
                new SearchFieldMutator<FilteredTransactionDto, TransactionSearchViewModel>(
                    search => search.SortColumn == "correspondent" && search.SortOrder == "asc",
                    (transactions, search) => transactions.OrderBy(u => u.Correspondent)),
                new SearchFieldMutator<FilteredTransactionDto, TransactionSearchViewModel>(
                    search => search.SortColumn == "correspondent" && search.SortOrder == "desc",
                    (transactions, search) => transactions.OrderByDescending(u => u.Correspondent)),

                // common paging
                new SearchFieldMutator<FilteredTransactionDto, TransactionSearchViewModel>(
                    //search => search.SortColumn != "correspondent",
                    search => true,
                    (transactions, search) => transactions.Skip(pageIndex * pageSize).Take(pageSize))
            };

            var searchModel = new TransactionSearchViewModel
            {
                ClientId = accountId,
                Correspondent = correspondent,
                Amount = amount,
                From = fromTransactionDate,
                To = toTransactionDate,
                SortColumn = sortColumn,
                SortOrder = sortOrder
            };

            // apply filters and sort
            var transactionsQuery = DbContext.AccountTransactions.AsQueryable();
            foreach (var searchFieldMutator in searchFieldMutators)
                transactionsQuery = searchFieldMutator.Apply(searchModel, transactionsQuery);

            // remember count for paginator after filtering
            var count = transactionsQuery.Count();
            // add correspondent name to dto
            var transactionsDtoQuery = from t in transactionsQuery
                join acc in DbContext.Accounts on t.CreditAccountId == accountId ? t.DebitAccountId : t.CreditAccountId
                equals acc.Id select new FilteredTransactionDto
                {
                    Amount = t.Amount,
                    TransactionDate = t.TransactionDate,
                    CorrespondentId = (t.CreditAccountId == accountId) ? t.DebitAccountId : t.CreditAccountId,
                    IsCredit = t.DebitAccountId == accountId,
                    Correspondent = acc.Name
                };

            // add correspondent sorting and paging
            foreach (var searchFieldMutator in searchFieldMutators2)
                transactionsDtoQuery = searchFieldMutator.Apply(searchModel, transactionsDtoQuery);

            return new FilteredTransactionListDto { Count = count, Transactions = transactionsDtoQuery.ToList() };
        }
    }
}
