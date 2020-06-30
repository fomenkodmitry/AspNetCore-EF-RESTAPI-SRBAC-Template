using System.IO;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Template
{
    public class EmailTemplateContainer
    {
        public EmailTemplateContainer(string language)
        {
            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                language,
                "Email"
            );

            var confirmEmail = File.ReadAllLines(Path.Combine(path, "ConfirmEmail.html"));
            var emailTemplate = new EmailTemplate
            {
                Theme = confirmEmail.First(),
                Text = string.Join("\n", confirmEmail.Skip(1))
            };
            ConfirmEmail = emailTemplate;
        }

        public EmailTemplate ConfirmEmail { get; set; }
    }

}