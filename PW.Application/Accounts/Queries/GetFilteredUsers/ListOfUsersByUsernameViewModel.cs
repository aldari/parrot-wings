using System.Collections.Generic;

namespace PW.Application.Accounts.Queries.GetFilteredUsers
{
    public class ListOfUsersByUsernameViewModel
    {
        public IEnumerable<UserDto> Users { get; set; }
    }
}
