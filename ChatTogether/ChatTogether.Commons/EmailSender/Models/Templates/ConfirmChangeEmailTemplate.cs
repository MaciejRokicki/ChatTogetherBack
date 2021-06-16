using ChatTogether.Commons.EmailSender.Templates;
using System.Collections.Generic;

namespace ChatTogether.Commons.EmailSender.Models.Templates
{
    public class ConfirmChangeEmailTemplate : MessageTemplate
    {
        public ConfirmChangeEmailTemplate(string email, string confirmationLink) : base(HtmlTemplates.ConfirmChangeEmailTemplate, new Dictionary<string, string>()
        {
            {
                "{{email}}", email
            },
            {
                "{{confirmationLink}}", confirmationLink
            }
        })
        { }
    }
}
