using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Localization
{
 public static class LocalizationManager
  {
    public static IDictionary<string, LanguageDescriptor> SupportedLanguages { get; } = (IDictionary<string, LanguageDescriptor>) new Dictionary<string, LanguageDescriptor>()
    {
      {
        "ru",
        new LanguageDescriptor(Language.Russian, new CultureInfo("ru-RU"))
      },
      {
        "en",
        new LanguageDescriptor(Language.English, new CultureInfo("en-US"))
      }
    };

    public static Language DefaultLanguage { get; } = Language.Russian;

    public static string DefaultLanguageCode { get; } = "ru";

    public static Language CurrentLanguage => LocalizationManager.SupportedLanguages.Where<KeyValuePair<string, LanguageDescriptor>>((Func<KeyValuePair<string, LanguageDescriptor>, bool>) (x => x.Value.CultureInfo == LocalizationManager.CurrentCulture)).Select<KeyValuePair<string, LanguageDescriptor>, Language>((Func<KeyValuePair<string, LanguageDescriptor>, Language>) (x => x.Value.Language)).DefaultIfEmpty<Language>(LocalizationManager.DefaultLanguage).FirstOrDefault<Language>();

    public static string CurrentLanguageCode => LocalizationManager.SupportedLanguages.Where<KeyValuePair<string, LanguageDescriptor>>((Func<KeyValuePair<string, LanguageDescriptor>, bool>) (x => x.Value.CultureInfo == LocalizationManager.CurrentCulture)).Select<KeyValuePair<string, LanguageDescriptor>, string>((Func<KeyValuePair<string, LanguageDescriptor>, string>) (x => x.Key)).DefaultIfEmpty<string>(LocalizationManager.DefaultLanguageCode).FirstOrDefault<string>();

    public static CultureInfo CurrentCulture => Thread.CurrentThread.CurrentCulture;

    public static Language GetLanguage(string language) => !LocalizationManager.SupportedLanguages.ContainsKey(language) ? LocalizationManager.DefaultLanguage : LocalizationManager.SupportedLanguages[language].Language;

    public static CultureInfo GetCultureInfo(Language language) => LocalizationManager.SupportedLanguages.First<KeyValuePair<string, LanguageDescriptor>>((Func<KeyValuePair<string, LanguageDescriptor>, bool>) (x => x.Value.Language == language)).Value.CultureInfo;

    public static void SetLanguage(Language language)
    {
      LanguageDescriptor languageDescriptor = LocalizationManager.SupportedLanguages.First<KeyValuePair<string, LanguageDescriptor>>((Func<KeyValuePair<string, LanguageDescriptor>, bool>) (x => x.Value.Language == language)).Value;
      Thread.CurrentThread.CurrentCulture = languageDescriptor.CultureInfo;
      Thread.CurrentThread.CurrentUICulture = languageDescriptor.CultureInfo;
    }

    public static void SetLanguage(string language) => LocalizationManager.SetLanguage(LocalizationManager.GetLanguage(language));
  }
}