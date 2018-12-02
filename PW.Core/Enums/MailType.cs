using System.ComponentModel.DataAnnotations;

namespace PW.Core.Enums
{
    public enum MailType
    {
        [Display(Name = "Регистрация суперюзера")]
        SuperUserRegistration,

        [Display(Name = "Регистрация юзера")]
        UserRegistration = 20,
        }
}
