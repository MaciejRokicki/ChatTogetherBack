using ChatTogether.Commons.EmailSender.Templates;
using System.Collections.Generic;

namespace ChatTogether.Commons.EmailSender.Models.Templates
{
    public class ChangePasswordRequestTemplate : MessageTemplate
    {
        public ChangePasswordRequestTemplate(string email, string confirmationLink) : base(HtmlTemplates.ChangePasswordRequestTemplate, new Dictionary<string, string>()
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
