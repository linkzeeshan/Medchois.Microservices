using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using UserManagementServices.Domain.Dtos.EmailDtos;
using UserManagementServices.Services.Interfaces;

namespace UserManagementServices.Services.EmailService
{
    public class UserEmailService : IUserEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public UserEmailService(IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }
        public async Task SendEmailAsyc(Message message)
        {
            var createEmail = CreateEmailMessage(message);
            await SendAsync(createEmail);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Medchois Confirmation Email", _emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using  (SmtpClient mailClient = new SmtpClient())
            {

                mailClient.Connect(_emailConfiguration.SmtpServer, int.Parse(_emailConfiguration.Port), MailKit.Security.SecureSocketOptions.StartTls);
                mailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                mailClient.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);
                await mailClient.SendAsync(mailMessage);
                mailClient.Disconnect(true);
            }

        }
    }
}
