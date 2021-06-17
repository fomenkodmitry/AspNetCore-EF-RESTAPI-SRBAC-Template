using System.Collections.Generic;

namespace Middleware.Options
{
    public class ContextProperties : IContextProperties
    {
        public IDictionary<string, object> Properties { get; } = (IDictionary<string, object>) new Dictionary<string, object>();

        public object Cookies
        {
            get => Get(nameof (Cookies));
            set => Set(nameof (Cookies), value);
        }

        public object CorrelationId
        {
            get => Get(nameof (CorrelationId));
            set => Set(nameof (CorrelationId), value);
        }

        public object Authorization
        {
            get => Get(nameof (Authorization));
            set => Set(nameof (Authorization), value);
        }

        public object XsrfToken
        {
            get => Get(nameof (XsrfToken));
            set => Set(nameof (XsrfToken), value);
        }

        public object Language
        {
            get => Get(nameof (Language));
            set => Set(nameof (Language), value);
        }

        public object UserId
        {
            get => Get(nameof (UserId));
            set => Set(nameof (UserId), value);
        }

        public object UserRole
        {
            get => Get(nameof (UserRole));
            set => Set(nameof (UserRole), value);
        }

        public object UserLogin
        {
            get => Get(nameof (UserLogin));
            set => Set(nameof (UserLogin), value);
        }

        private object Get(string propertyName)
        {
            object obj;
            Properties.TryGetValue(propertyName, out obj);
            return obj;
        }

        private void Set(string propertyName, object value) => Properties[propertyName] = value;
    }
}