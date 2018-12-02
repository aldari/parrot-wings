using System.Threading.Tasks;

namespace PW.Core.Conventions
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
