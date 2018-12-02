using System.Threading.Tasks;
using PW.Core.Enums;

namespace PW.Core.Conventions
{
    public interface INotificationSender
    {
       Task SendMail(string to, string subj, MailType mailType, object model = null);
    }
}
