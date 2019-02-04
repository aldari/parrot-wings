using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PW.Application.Accounts.Queries.GetAccountLastTransactions;
using PW.Application.Accounts.Queries.GetAccountTransactions;
using PW.Core.Account.Command;
using PW.Core.Account.Query;
using PW.DataAccess.Account.Command;
using PW.Hubs;
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
        private readonly NotifyHub _notifyHub;
        private readonly IMediator _mediator;

        public TransactionController(AddTransactionCommandHandler addTransactionCommandHandler,
            IMediator mediator)
        {
            _addTransactionCommandHandler = addTransactionCommandHandler;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetTransactions([FromQuery] AccountTransactionsQuery query)
        {
            query.AccountId = Guid.Parse(User.FindFirst("AccountId").Value);
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("last")]
        public async Task<ActionResult> GetLastTransactions()
        {
            var accountId = Guid.Parse(User.FindFirst("AccountId").Value);
            var query = new AccountLastTransactionQuery {AccountId = accountId };
            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<ActionResult> AddTransactionAsync([FromBody] TransactionModel model)
        {
            var accountId = Guid.Parse(User.FindFirst("AccountId").Value);
            var command = new AddTransactionCommand(accountId, model.Recipient, model.Amount);
            var result = await _addTransactionCommandHandler.ExecuteAsync(command);
            if (result.Success)
            {
                await _notifyHub.Notify("sherlock.holmes@gmail.com", model.Amount.ToString());
                return Ok();
            }
            return BadRequest(result);
        }
    }
}