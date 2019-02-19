using MediatR;
using System;

namespace PW.Application.Accounts.Queries.GetAccountBalance
{
    public class AccountBalanceQuery : IRequest<int>
    {
        public Guid AccountId { get; set; }
    }
}
