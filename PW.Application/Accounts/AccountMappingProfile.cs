using AutoMapper;
using PW.Application.Accounts.Queries.GetFilteredUsers;
using PW.Domain.Entities;

namespace PW.Application.Accounts
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<Account, UserDto>();
        }
    }
}
