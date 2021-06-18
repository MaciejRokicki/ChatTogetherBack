using ChatTogether.Commons.EmailSender.Models.Templates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Commons.EmailSender
{
    public interface IEmailSender
    {
        Task Send(string recipientEmail, MessageTemplate messageTemplate);
        Task SendMany(IEnumerable<string> recipientEmails, MessageTemplate messageTemplate);
    }
}
