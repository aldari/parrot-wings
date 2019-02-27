using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PW.Domain.Entities;
using PW.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PW.Application.Accounts.Queries.GetAccountTransactions
{
    public class AccountTransactionsQueryHandler : IRequestHandler<AccountTransactionsQuery, FilteredTransactionListDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AccountTransactionsQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FilteredTransactionListDto> Handle(AccountTransactionsQuery request, CancellationToken cancellationToken)
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
                    (transactions, search) => transactions.Skip(request.PageIndex * request.PageSize).Take(request.PageSize))
            };

            var searchModel = new TransactionSearchViewModel
            {
                ClientId = request.AccountId,
                Correspondent = request.Correspondent,
                Amount = request.Amount,
                From = request.From,
                To = request.To,
                SortColumn = request.SortColumn,
                SortOrder = request.SortOrder
            };

            // apply filters and sort
            var transactionsQuery = _context.AccountTransactions.AsNoTracking();
            foreach (var searchFieldMutator in searchFieldMutators)
                transactionsQuery = searchFieldMutator.Apply(searchModel, transactionsQuery);

            // remember count for paginator after filtering
            var count = await transactionsQuery.CountAsync();
            // add correspondent name to dto
            var transactionsDtoQuery = from t in transactionsQuery
                                       join acc in _context.Accounts on t.CreditAccountId == request.AccountId ? t.DebitAccountId : t.CreditAccountId
                                       equals acc.Id
                                       select new FilteredTransactionDto
                                       {
                                           Id = t.Id,
                                           Amount = t.Amount,
                                           TransactionDate = t.TransactionDate,
                                           CorrespondentId = (t.CreditAccountId == request.AccountId) ? t.DebitAccountId : t.CreditAccountId,
                                           IsCredit = t.DebitAccountId == request.AccountId,
                                           Correspondent = acc.Name
                                       };

            // add correspondent sorting and paging
            foreach (var searchFieldMutator in searchFieldMutators2)
                transactionsDtoQuery = searchFieldMutator.Apply(searchModel, transactionsDtoQuery);

            return new FilteredTransactionListDto { Count = count, Transactions = await transactionsDtoQuery.ToListAsync() };
        }
    }
}
