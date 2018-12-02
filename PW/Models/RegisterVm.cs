using System.ComponentModel.DataAnnotations;

namespace PW.Models
{
    public class RegisterVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
