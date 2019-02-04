using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PW.Core.Account.Domain;
using PW.DataAccess.ApplicationData;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PW.Application.Accounts.Queries.GetFilteredUsers
{
    public class FilteredUsersQueryHandler : IRequestHandler<FilteredUsersQuery, ListOfUsersByUsernameViewModel>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FilteredUsersQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListOfUsersByUsernameViewModel> Handle(FilteredUsersQuery request, CancellationToken cancellationToken)
        {
            var list = await _context
                .Accounts
                .Where(x => x.Name.Contains(request.NameFilter)
                    && x.Id != AccountConst.SystemAccountGuid
                    // do not display the account owner to avoid sending funds to yourself
                    && x.Id != request.AccountId)
                .OrderBy(x => x.Name)
                .Take(4)
                .ToListAsync();

            var result = new ListOfUsersByUsernameViewModel
            {
                Users = _mapper.Map<List<UserDto>>(list)
            };
            return result;
        }
    }
}
