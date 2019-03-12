using System.ComponentModel.DataAnnotations;

namespace PW.Models
{
    public class CredentialsViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
