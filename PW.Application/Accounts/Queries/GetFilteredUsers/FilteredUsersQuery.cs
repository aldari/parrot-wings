using MediatR;
using System;

namespace PW.Application.Accounts.Queries.GetFilteredUsers
{
    public class FilteredUsersQuery : IRequest<ListOfUsersByUsernameViewModel>
    {
        public string NameFilter { get; set; }

        public Guid AccountId { get; set; }
    }
}
