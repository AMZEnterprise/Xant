using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Xant.MVC.Services.EmailSender
{
    /// <summary>
    /// Default implementation for IEmailSender.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _options;
        public EmailSender(IOptionsSnapshot<EmailSenderOptions> options)
        {
            _options = new EmailSenderOptions()
            {
                UserName = options.Value.UserName,
                Password = options.Value.Password,
                SmtpHost = options.Value.SmtpHost,
                SmtpPort = options.Value.SmtpPort
            };
        }
        public bool SendEmail(string emailAddress, string subject, string message)
        {
            string userName = _options.UserName;
            string password = _options.Password;
            string smtpHost = _options.SmtpHost;
            int smtpPort = _options.SmtpPort;

            SmtpClient client = new SmtpClient(smtpHost, smtpPort)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(userName)
            };

            mailMessage.To.Add(emailAddress);
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
