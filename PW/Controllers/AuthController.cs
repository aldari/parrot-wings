using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using PW.Core.Account.Domain;
using PW.Models;
using PW.Infrastructure;
using PW.Data;
using PW.DataAccess.ApplicationData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        public AuthController(UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions,
           ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _userManager.UserValidators.Clear();
            _userManager.UserValidators.Add(new CustomUserValidator<ApplicationUser>(applicationDbContext));
            _jwtFactory = jwtFactory;
            _applicationDbContext = applicationDbContext;
            _jwtOptions = jwtOptions.Value;

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        // POST api/auth/login
        [AllowAnonymous]
        [HttpPost("login")]
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

            var roles = await _userManager.GetRolesAsync(user);
            var isAdministrator = roles.Contains(RolesList.AdministratorRole);
            var isSuperuser = roles.Contains(RolesList.SuperuserRole);
            var accountId = _applicationDbContext.Accounts.Single(x => x.UserId.ToString() == user.Id).Id;
            // Serialize and return the response
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await _jwtFactory.GenerateEncodedToken(credentials.Email, identity, isAdministrator, isSuperuser, 
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
                // var userToVerify = await _userManager.FindByNameAsync(userName);
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
    }
}