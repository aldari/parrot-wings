using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PW.Core.Account.Command;
using PW.Core.Account.Domain;
using PW.DataAccess.Account.Command;
using PW.Models;
using System;
using System.Threading.Tasks;
using PW.DataAccess.ApplicationData;
using PW.Infrastructure;

namespace PW.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly AddAccountCommandHandler _accountCommandHandler;

        public RegisterController(UserManager<ApplicationUser> userManager, 
            IMapper mapper,
            AddAccountCommandHandler accountCommandHandler,
            ApplicationDbContext applicationDbContext
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _userManager.UserValidators.Clear();
            _userManager.UserValidators.Add(new CustomUserValidator<ApplicationUser>(applicationDbContext));
            _accountCommandHandler = accountCommandHandler;
        }
        
        [AllowAnonymous]
        [HttpPost]
        //[Route("register")]
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
                await _accountCommandHandler.ExecuteAsync(
                    new AddAccountCommand(Guid.Parse(user.Id), userModel.FullName));
            }

            IActionResult errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok(new { user.Email, user.UserName });
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> GetUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return Ok(_mapper.Map<UserVm>(user));
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
