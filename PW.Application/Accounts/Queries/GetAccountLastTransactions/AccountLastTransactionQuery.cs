using MediatR;
using System;
using System.Collections.Generic;

namespace PW.Application.Accounts.Queries.GetAccountLastTransactions
{
    public class AccountLastTransactionQuery : IRequest<List<LastTransactionDto>>
    {
        public Guid AccountId { get; set; }
    }
}
