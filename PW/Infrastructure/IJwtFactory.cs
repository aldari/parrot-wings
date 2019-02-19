using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PW.Infrastructure
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity, string fullName, Guid accountId);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}
