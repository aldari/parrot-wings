using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PW.Core.Account.Query;
using System;
using System.Threading.Tasks;

namespace PW.Controllers
{
    [Authorize]
   // [Produces("application/json")]
    [Route("api/recipient")]
    public class RecipientController : Controller
    {
        private readonly IGetFilteredUsersQuery _filteredUsersQuery;

        public RecipientController(IGetFilteredUsersQuery filteredUsersQuery)
        {
            _filteredUsersQuery = filteredUsersQuery;
        }

        /// <summary>
        /// Returns a list of registered users .
        /// </summary>
        /// <remarks>
        /// Here is a sample remarks placeholder.
        /// </remarks>
        /// <param name="nameFilter">The first name to search for</param>
        /// <returns>A string status</returns>
        [HttpGet("{nameFilter}")]
        public async Task<ActionResult> Get(string nameFilter)
        {
            var accountId = Guid.Parse(User.FindFirst("AccountId").Value);
            var result = await _filteredUsersQuery.Execute(nameFilter, accountId);
            return Ok(result);
        }
    }
}