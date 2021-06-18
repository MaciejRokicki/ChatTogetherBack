using ChatTogether.Commons.EmailSender.Templates;
using System.Collections.Generic;

namespace ChatTogether.Commons.EmailSender.Models.Templates
{
    public class ChangeEmailRequestTemplate : MessageTemplate
    {
        public ChangeEmailRequestTemplate(string email, string confirmationLink) : base("Prośba o zmiane adresu email", HtmlTemplates.ChangeEmailRequestTemplate, new Dictionary<string, string>()
        {
            {
                "{{email}}", email
            },
            {
                "{{confirmationLink}}", confirmationLink
            }
        }) { }
    }
}
