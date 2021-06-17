using System;
using System.Collections.Generic;
using System.Resources;
using Exceptions.Base;
using Localization;

namespace Exceptions.Localized
{
    public class LocalizedAuthorizationException : AuthorizationException
    {
        public LocalizedAuthorizationException(
            string code,
            ResourceManager resourceManager,
            IDictionary<string, object> errorArguments = null)
            : base(code, LocalizationResourceFormatter.GetLocalizedString(resourceManager, code), errorArguments)
        {
        }

        public LocalizedAuthorizationException(
            string code,
            ResourceManager resourceManager,
            params object[] resourceArguments)
            : base(code, LocalizationResourceFormatter.GetLocalizedString(resourceManager, code, resourceArguments))
        {
        }

        public LocalizedAuthorizationException(
            string code,
            ResourceManager resourceManager,
            IDictionary<string, object> errorArguments,
            params object[] resourceArguments)
            : base(code, LocalizationResourceFormatter.GetLocalizedString(resourceManager, code, resourceArguments), errorArguments)
        {
        }

        public LocalizedAuthorizationException(
            string code,
            Exception innerException,
            ResourceManager resourceManager,
            IDictionary<string, object> errorArguments,
            params object[] resourceArguments)
            : base(code, LocalizationResourceFormatter.GetLocalizedString(resourceManager, code, resourceArguments), innerException, errorArguments)
        {
        }
    }
}