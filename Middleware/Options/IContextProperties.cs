using System.Collections.Generic;

namespace Middleware.Options
{
    public interface IContextProperties
    {
        IDictionary<string, object> Properties { get; }

        object Cookies { get; set; }

        object CorrelationId { get; set; }

        object Authorization { get; set; }

        object XsrfToken { get; set; }

        object Language { get; set; }

        object UserId { get; set; }

        object UserRole { get; set; }

        object UserLogin { get; set; }
    }
}