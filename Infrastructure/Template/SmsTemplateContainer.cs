using System.IO;
using System.Reflection;

namespace Infrastructure.Template
{
    public class SmsTemplateContainer
    {
        public SmsTemplateContainer(string language)
        {
            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                language,
                "SMS"
            );

            Confirm = File.ReadAllText(Path.Combine(path, "Confirm.txt"));
        }

        public string Confirm { get; set; }
    }
}