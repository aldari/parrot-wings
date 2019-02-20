using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PW.Application.Accounts.Queries.GetAccountBalance;
using PW.Domain.Entities;
using PW.Infrastructure;
using PW.Models;
using PW.Persistence;
using System;
using System.Threading.Tasks;

namespace PW.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public RegisterController(UserManager<ApplicationUser> userManager, 
            ApplicationDbContext applicationDbContext,
            IMediator mediator
            )
        {
            _userManager = userManager;
            _userManager.UserValidators.Clear();
            _userManager.UserValidators.Add(new CustomUserValidator<ApplicationUser>(applicationDbContext));
            _mediator = mediator;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterVm userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                Email = userModel.Email,
                UserName = userModel.Email,
                FullName = userModel.FullName
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            // Подтверждение почты пользователя
            string confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
            var test = _userManager.ConfirmEmailAsync(user, confirmationToken).Result;
            if (result.Succeeded)
            {
                var request = new Application.Accounts.Commands.AddAccount.AddAccountCommand
                {
                    Name = userModel.FullName,
                    UserId = Guid.Parse(user.Id)
                };
                await _mediator.Send(request);
            }

            IActionResult errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok(new { user.Email, user.UserName });
        }

        [HttpGet("balance")]
        [Authorize]
        public async Task<OkObjectResult> GetUserBalance()
        {
            var accountId = Guid.Parse(User.FindFirst("AccountId").Value);            
            return Ok(await _mediator.Send(new AccountBalanceQuery{ AccountId = accountId }));
        }

        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return BadRequest();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
