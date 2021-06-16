using ChatTogether.Commons.EmailSender.Templates;
using System.Collections.Generic;

namespace ChatTogether.Commons.EmailSender.Models.Templates
{
    public class ChangeEmailRequestTemplate : MessageTemplate
    {
        public ChangeEmailRequestTemplate(string email, string newEmail, string confirmationLink) : base(HtmlTemplates.ChangeEmailRequestTemplate, new Dictionary<string, string>()
        {
            {
                "{{email}}", email
            },
            {
                "{{newEmail}}", newEmail
            },
            {
                "{{confirmationLink}}", confirmationLink
            }
        }) { }
    }
}
