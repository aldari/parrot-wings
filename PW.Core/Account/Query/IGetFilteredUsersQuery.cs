using PW.Core.Account.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PW.Core.Account.Query
{
    public interface IGetFilteredUsersQuery
    {
        Task<List<UserDto>> Execute(string userNameFilter, Guid exceptAccountId);
    }
}
