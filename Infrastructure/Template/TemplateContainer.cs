using System;
using Domain.i18n;
using System.Collections.Generic;

namespace Infrastructure.Template
{
    public class TemplateContainer
    {
        public Languages DefaultLanguage = Languages.Russian;

        public Dictionary<Languages, SmsTemplateContainer> SMS { get; set; } = new Dictionary<Languages, SmsTemplateContainer>();
        public Dictionary<Languages, EmailTemplateContainer> Email { get; set; } = new Dictionary<Languages, EmailTemplateContainer>();
        public Dictionary<Languages, PushTemplateContainer> Push { get; set; } = new Dictionary<Languages, PushTemplateContainer>();

        public TemplateContainer()
        { 
            var sLang =  new List<Languages> { Languages.Russian };
            foreach (var language in sLang)
            {
                var lang = Enum.GetName(typeof(Languages), language)?.Substring(0,2).ToLower();
                SMS[language] = new SmsTemplateContainer(lang);
                Email[language] = new EmailTemplateContainer(lang);
                Push[language] = new PushTemplateContainer(lang);
            }
        }
    }
}