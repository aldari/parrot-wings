using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PW.Application.Accounts.Commands.AddTransaction;
using PW.Application.Accounts.Queries.GetAccountLastTransactions;
using PW.Application.Accounts.Queries.GetAccountTransactions;
using PW.Models;
using System;
using System.Threading.Tasks;

namespace PW.Api.Controllers
{
    [Authorize]
    [Route("api/transaction")]
    public class TransactionController : Controller
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
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
            var command = new AddTransactionCommand
            {
                DebitAccount = model.Recipient,
                CreditAccount = accountId,
                Amount = model.Amount
            };
            var result = await _mediator.Send(command);
            if (result.Success)
            {
                return Ok();
            }
            return BadRequest(result);
        }
    }
}