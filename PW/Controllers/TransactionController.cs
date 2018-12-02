using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PW.Core.Account.Command;
using PW.Core.Account.Query;
using PW.DataAccess.Account.Command;
using PW.Models;
using System;
using System.Threading.Tasks;

namespace PW.Api.Controllers
{
    [Authorize]
    [Route("api/transaction")]
    public class TransactionController : Controller
    {
        private readonly AddTransactionCommandHandler _addTransactionCommandHandler;
        private readonly IGetFilteredAccountTransactionsQuery _getAccountTransactionsQuery;
        private readonly IGetLastAccountTransactionsQuery _getLastAccountTransactionsQuery;

        public TransactionController(AddTransactionCommandHandler addTransactionCommandHandler,
            IGetFilteredAccountTransactionsQuery getAccountTransactionsQuery,
            IGetLastAccountTransactionsQuery getLastAccountTransactionsQuery)
        {
            _addTransactionCommandHandler = addTransactionCommandHandler;
            _getAccountTransactionsQuery = getAccountTransactionsQuery;
            _getLastAccountTransactionsQuery = getLastAccountTransactionsQuery;
        }

        [HttpGet]
        public async Task<ActionResult> GetTransactions(int? amount, Guid correspondent, DateTime? from, DateTime? to,
            string sortColumn, string sortOrder = "asc", int pageIndex = 0, int pageSize = 3)
        {
            var accountId = Guid.Parse(User.FindFirst("AccountId").Value);
            return Ok(await _getAccountTransactionsQuery.Execute(sortColumn, sortOrder, accountId, pageIndex,
                pageSize, amount, from, to, correspondent));
        }

        [HttpGet("last")]
        public async Task<ActionResult> GetLastTransactions()
        {
            var accountId = Guid.Parse(User.FindFirst("AccountId").Value);
            return Ok(await _getLastAccountTransactionsQuery.Execute(accountId));
        }

        [HttpPost]
        public async Task<ActionResult> AddTransactionAsync([FromBody] TransactionModel model)
        {
            var accountId = Guid.Parse(User.FindFirst("AccountId").Value);
            var command = new AddTransactionCommand(accountId, model.Recipient, model.Amount);
            var result = await _addTransactionCommandHandler.ExecuteAsync(command);
            if (result.Success)
                return Ok();
            return BadRequest(result);
        }
    }
}