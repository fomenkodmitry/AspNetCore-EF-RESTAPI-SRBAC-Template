using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Infrastructure.Template
{
    public class PushTemplateContainer
    {
        public PushTemplateContainer(string language)
        {
            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                language, 
                "Push"
            );
            Test = JsonConvert.DeserializeObject<PushTemplate>(File.ReadAllText(Path.Combine(path, "Test.json")));
        }
        public PushTemplate Test { get; set; }
    }
}