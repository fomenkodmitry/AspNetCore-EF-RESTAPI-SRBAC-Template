using System;
using System.Collections.Generic;
using System.Resources;
using Exceptions.Base;
using Localization;

namespace Exceptions.Localized
{
    public class LocalizedBusinessLogicException : BusinessLogicException
    {
        public LocalizedBusinessLogicException(
            string code,
            ResourceManager resourceManager,
            IDictionary<string, object> errorArguments = null)
            : base(code, LocalizationResourceFormatter.GetLocalizedString(resourceManager, code), errorArguments)
        {
        }

        public LocalizedBusinessLogicException(
            string code,
            ResourceManager resourceManager,
            params object[] resourceArguments)
            : base(code, LocalizationResourceFormatter.GetLocalizedString(resourceManager, code, resourceArguments))
        {
        }

        public LocalizedBusinessLogicException(
            string code,
            ResourceManager resourceManager,
            IDictionary<string, object> errorArguments,
            params object[] resourceArguments)
            : base(code, LocalizationResourceFormatter.GetLocalizedString(resourceManager, code, resourceArguments), errorArguments)
        {
        }

        public LocalizedBusinessLogicException(
            string code,
            Exception innerException,
            ResourceManager resourceManager,
            IDictionary<string, object> errorArguments,
            params object[] resourceArguments)
            : base(code, LocalizationResourceFormatter.GetLocalizedString(resourceManager, code, resourceArguments), innerException, errorArguments)
        {
        }

        public LocalizedBusinessLogicException(ErrorDetails errorDetails)
            : base(errorDetails)
        {
        }
    }
}