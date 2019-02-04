using Microsoft.AspNetCore.Identity;

namespace PW.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
