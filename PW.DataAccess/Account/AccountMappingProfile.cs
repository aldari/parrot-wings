using AutoMapper;
using PW.Core.Account.Dto;

namespace PW.DataAccess.Account
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<Core.Account.Domain.Account, UserDto>();
        }
    }
}
