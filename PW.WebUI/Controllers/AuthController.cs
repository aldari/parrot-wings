using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PW.Domain.Entities;
using PW.Infrastructure;
using PW.Models;
using PW.Persistence;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PW.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IMediator _mediator;

        public AuthController(UserManager<ApplicationUser> userManager,
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions,
            ApplicationDbContext applicationDbContext,
            IMediator mediator)
        {
            _userManager = userManager;
            _userManager.UserValidators.Clear();
            _userManager.UserValidators.Add(new CustomUserValidator<ApplicationUser>(applicationDbContext));
            _jwtFactory = jwtFactory;
            _applicationDbContext = applicationDbContext;
            _jwtOptions = jwtOptions.Value;
            _mediator = mediator;

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        // POST api/auth/token
        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> Post([FromBody]CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.Email, credentials.Password);
            var user = await _applicationDbContext.Users.SingleOrDefaultAsync(x => x.Email == credentials.Email);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            var accountId = _applicationDbContext.Accounts.Single(x => x.UserId.ToString() == user.Id).Id;
            // Serialize and return the response
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await _jwtFactory.GenerateEncodedToken(credentials.Email, identity, 
                    user.FullName, accountId),
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                var userToVerify = _applicationDbContext.Users.SingleOrDefault(x => x.Email == email);

                if (userToVerify != null)
                {
                    // check the credentials  
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(email, userToVerify.Id));
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        // POST api/auth
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