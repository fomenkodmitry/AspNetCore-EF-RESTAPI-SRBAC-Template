using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace Localization
{
    public static class LocalizationResourceFormatter
    {
        public static string GetLocalizedString(
            ResourceManager manager,
            string key,
            params object[] args)
        {
            string format = manager.GetString(key, LocalizationManager.CurrentCulture);
            return !args.Any() ? format : string.Format(format, args);
        }

        public static string GetLocalizedString(
            ResourceManager manager,
            Language language,
            string key,
            params object[] args)
        {
            string format = manager.GetString(key, LocalizationManager.GetCultureInfo(language));
            return !args.Any() ? format : string.Format(format, args);
        }
    }
}