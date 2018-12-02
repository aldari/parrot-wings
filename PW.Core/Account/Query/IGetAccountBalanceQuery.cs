using System;
using PW.Core.Account.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PW.Core.Account.Query
{
    public interface IGetAccountBalanceQuery
    {
        Task<int> Execute(Guid accountId);
    }
}
