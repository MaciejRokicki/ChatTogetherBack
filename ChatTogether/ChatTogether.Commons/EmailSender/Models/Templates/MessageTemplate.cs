using System.Collections.Generic;
using System.Text;

namespace ChatTogether.Commons.EmailSender.Models.Templates
{
    public abstract class MessageTemplate
    {
        public string HtmlTemplate { get; }
        private Dictionary<string, string> Placeholders { get; set; }

        public MessageTemplate(string htmlTemplate, Dictionary<string, string> placeholders)
        {
            HtmlTemplate = htmlTemplate;
            Placeholders = placeholders;
            HtmlTemplate = ReplacePlaceholders();
        }

        private string ReplacePlaceholders()
        {
            StringBuilder stringBuilder = new StringBuilder(HtmlTemplate);

            foreach (KeyValuePair<string, string> placeholder in Placeholders)
            {
                stringBuilder.Replace(placeholder.Key, placeholder.Value);
            }

            return stringBuilder.ToString();
        }
    }
}
