using System.Threading.Tasks;
using PW.Core.Conventions;
using PW.Core.Enums;

namespace PW.DataAccess.ExternalServices
{
    public class NotificationSender : INotificationSender
    {
        private readonly IEmailSender _emailService;
        private readonly IViewRenderService _viewRenderService;

        public NotificationSender(IEmailSender emailService
            , IViewRenderService viewRenderService)
        {
            _emailService = emailService;
            _viewRenderService = viewRenderService;
        }

        public async Task SendMail(string email, string caption, MailType mailType, object model = null)
        {
            string emailText = await _viewRenderService.RenderToStringAsync($"EmailTemplates/_{mailType}", model);
            await _emailService.SendEmailAsync(email, caption, emailText);
        }
    }
}
