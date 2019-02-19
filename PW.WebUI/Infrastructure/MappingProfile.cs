using AutoMapper;
using PW.Domain.Entities;
using PW.Models;

namespace PW.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserVm>();
        }
    }
}
