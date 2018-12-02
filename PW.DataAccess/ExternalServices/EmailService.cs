using System.Threading.Tasks;
using PW.Core.AppSettings;
using PW.Core.Conventions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace PW.DataAccess.ExternalServices
{
        public class EmailService : IEmailSender
    {
        private readonly EmailServiceOptions _options;

        public EmailService(IOptions<EmailServiceOptions> emailServiceOptions)
        {
            _options = emailServiceOptions.Value;
        }


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("PW", _options.SenderLogin));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_options.Host, _options.Port, _options.UseSsl);
                await client.AuthenticateAsync(_options.SenderLogin, _options.SenderPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
