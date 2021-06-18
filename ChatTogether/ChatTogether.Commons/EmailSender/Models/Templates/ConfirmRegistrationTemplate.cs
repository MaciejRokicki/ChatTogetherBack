using ChatTogether.Commons.EmailSender.Templates;
using System.Collections.Generic;

namespace ChatTogether.Commons.EmailSender.Models.Templates
{
    public class ConfirmRegistrationTemplate : MessageTemplate
    {
        public ConfirmRegistrationTemplate(string email, string confirmationLink) : base("Potwierdzenie adresu email", HtmlTemplates.ConfirmRegistrationTemplate, new Dictionary<string, string>()
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
