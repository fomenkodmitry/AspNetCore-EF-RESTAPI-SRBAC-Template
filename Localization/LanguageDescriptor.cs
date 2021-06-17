using System.Globalization;

namespace Localization
{
    public class LanguageDescriptor
    {
        public Language Language { get; set; }

        public CultureInfo CultureInfo { get; set; }

        public LanguageDescriptor()
        {
        }

        public LanguageDescriptor(Language language, CultureInfo cultureInfo)
        {
            this.Language = language;
            this.CultureInfo = cultureInfo;
        }
    }
}