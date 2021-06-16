using ChatTogether.Commons.EmailSender.Models.Templates;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ChatTogether.Commons.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration emailConfiguration;

        public EmailSender(IOptions<EmailConfiguration> emailConfiguration)
        {
            this.emailConfiguration = emailConfiguration.Value;
        }

        public async Task Send(string recipientEmail, string subject, MessageTemplate messageTemplate)
        {
            using (SmtpClient smtpClient = new SmtpClient(emailConfiguration.Host, emailConfiguration.Port))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailConfiguration.Username, emailConfiguration.Password);
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                using (MailMessage message = new MailMessage(emailConfiguration.Username, recipientEmail, subject, messageTemplate.HtmlTemplate))
                {
                    message.IsBodyHtml = true;
                    await smtpClient.SendMailAsync(message);
                }
            }
        }

        public async Task SendMany(IEnumerable<string> recipientEmails, string subject, MessageTemplate messageTemplate)
        {
            foreach (string email in recipientEmails)
            {
                await Send(email, subject, messageTemplate);
            }
        }
    }
}
