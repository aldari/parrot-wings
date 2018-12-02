using AutoMapper;
using System;
using PW.Core.Account.Domain;
using PW.Models;
using PW.Core.Extensions;

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
