using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using PatientManagementServices.Application.Common;
using PatientManagementServices.Application.Core.Background;
using PatientManagementServices.Application.Core.Notification;
using MailKit.Net.Smtp;
using PatientManagementServices.Domain.Interfaces;

namespace PatientManagementServices.Infrastructure.Notificaion
{
    public class NotificationMsg : INotificationMsg
    {
        private readonly IBackgroundJob _backgroundJob;

        public NotificationMsg(IBackgroundJob backgroundJob)
        {
            _backgroundJob = backgroundJob;
        }

        public void SaveMailNotification(string From, string To, string Subject, string Body)
        {
            Notfication notfication = new()
            {
                MsgFrom = From,
                MsgTo = To,
                MsgSubject = Subject,
                MsgBody = Body,
                MsgSatus = NotificationStatus.Pending.ToString(),
                MsgType = NotificationType.Mail.ToString(),
                CreatedDate = System.DateTime.Now,
            };
            //_backgroundJob.AddEnque<IDapper<Notfication>>(x => x.SaveNotificationAsync(notfication));
        }

        public async Task<bool> SendAsync(Notfication notfication)
        {
            switch (notfication.MsgType.ToString())
            {
                case "Mail":
                    return await SendMailAsync(notfication);

                case "SMS":
                    return true;

                case "Whatsapp":
                    return true;

                default:
                    return true;
            }
        }

        private async Task<bool> SendMailAsync(Notfication notfication)
        {
            try
            {
                // create email message
                MimeMessage email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(notfication.MsgFrom));
                email.To.Add(MailboxAddress.Parse(notfication.MsgTo));
                email.Subject = notfication.MsgSubject;
                email.Body = new TextPart(TextFormat.Html) { Text = notfication.MsgBody };

                // send email
                using SmtpClient smtp = new SmtpClient();
                smtp.Connect(APISetting.EmailConfiguration.SMTPAddress, APISetting.EmailConfiguration.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(APISetting.EmailConfiguration.UserId, APISetting.EmailConfiguration.Password);
                smtp.Send(email);
                smtp.Disconnect(true);

                notfication.MsgSatus = NotificationStatus.Success.ToString();
                notfication.UpdatedDate = DateTime.Now;
                //await _repository.UpdateNotificationAsync(notfication);
                return true;
            }
            catch (Exception ex)
            {
                notfication.MsgSatus = NotificationStatus.Fail.ToString();
                notfication.UpdatedDate = DateTime.Now;
                if (ex.InnerException != null)
                {
                    notfication.FailDetails = ex.Message + ex.InnerException.Message;
                }
                else
                {
                    notfication.FailDetails = ex.Message;
                }

                //await _repository.UpdateNotificationAsync(notfication);
                return false;
            }
        }
    }
}
