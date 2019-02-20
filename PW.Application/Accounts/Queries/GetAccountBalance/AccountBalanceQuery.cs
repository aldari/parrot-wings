using MediatR;
using System;

namespace PW.Application.Accounts.Queries.GetAccountBalance
{
    public class AccountBalanceQuery : IRequest<BalanceDto>
    {
        public Guid AccountId { get; set; }
    }
}
