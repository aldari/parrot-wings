using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PW.Core.Account.Domain;
using PW.Core.Account.Dto;
using PW.Core.Account.Query;
using PW.DataAccess.ApplicationData;
using PW.DataAccess.Cqs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.DataAccess.Account.Query
{
    public class GetFilteredUsersQuery : EfQueryBase<ApplicationDbContext>, IGetFilteredUsersQuery
    {
        private readonly IMapper _mapper;

        public GetFilteredUsersQuery(ApplicationDbContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Execute(string userNameFilter, Guid exceptAccountId)
        {
            var list = await DbContext
                .Accounts
                .Where(x => x.Name.Contains(userNameFilter.Replace("*", "")) 
                    && x.Id != AccountConst.SystemAccountGuid 
                    && x.Id != exceptAccountId)
                .OrderBy(x => x.Name)
                .Take(4)
                .ToListAsync();

            var result = _mapper.Map<List<UserDto>>(list);
            return result;
        }
    }
}
