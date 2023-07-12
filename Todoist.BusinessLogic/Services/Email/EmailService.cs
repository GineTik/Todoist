using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Todoist.BusinessLogic.Options;

namespace Todoist.BusinessLogic.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOption;

        public EmailService(IOptions<EmailOptions> emailOption)
        {
            _emailOption = emailOption.Value;
        }

        public async Task SendConfirmationEmailAsync(string email, string code)
        {
            await SendEmailAsync(
                email, 
                "Confirmation email",
                $"Code: {code}");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient();
            client.Port = _emailOption.Port;
            client.Host = _emailOption.Host;
            client.EnableSsl = _emailOption.EnableSSL;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_emailOption.Credentials.UserName, _emailOption.Credentials.Password);

            await client.SendMailAsync(_emailOption.Credentials.UserName, email, subject, htmlMessage);
        }
    }
}
