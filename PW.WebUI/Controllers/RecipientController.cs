using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PW.Application.Accounts.Queries.GetFilteredUsers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PW.Controllers
{
    [Authorize]
    [Route("api/recipient")]
    public class RecipientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecipientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns a list of registered users.
        /// </summary>
        /// <remarks>
        /// Here is a sample remarks placeholder.
        /// </remarks>
        /// <param name="nameFilter">The first name to search for</param>
        /// <returns>A string status</returns>
        [HttpGet("{nameFilter}")]
        [ProducesResponseType(typeof(ListOfUsersByUsernameViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromRoute] FilteredUsersQuery query)
        {
            var accountId = Guid.Parse(User.FindFirst("AccountId").Value);
            query.AccountId = accountId;
            return Ok(await _mediator.Send(query));
        }
    }
}