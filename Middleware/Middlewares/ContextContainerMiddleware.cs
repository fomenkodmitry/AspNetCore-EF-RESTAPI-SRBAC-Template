using System.Threading.Tasks;
using Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Middleware.Options;

namespace Middleware.Middlewares
{
    public class ContextContainerMiddleware
    {
        private readonly RequestDelegate _next;

        public ContextContainerMiddleware(RequestDelegate next) => this._next = next;

        public async Task Invoke(HttpContext context)
        {
            ContextProperties contextProperties = new ContextProperties();
            StringValues stringValues1;
            context.Request.Headers.TryGetValue(HeaderNames.Cookie, out stringValues1);
            contextProperties.Cookies = stringValues1;
            StringValues stringValues2;
            context.Request.Headers.TryGetValue(CorrelationIdOptions.DefaultHeader, out stringValues2);
            contextProperties.CorrelationId = stringValues2;
            StringValues stringValues3;
            context.Request.Headers.TryGetValue(HeaderNames.Authorization, out stringValues3);
            contextProperties.Authorization = stringValues3;
            StringValues stringValues4;
            context.Request.Headers.TryGetValue("X-XSRF-Request-Token", out stringValues4);
            contextProperties.XsrfToken = stringValues4;
            contextProperties.Language = LocalizationManager.CurrentLanguageCode;
            contextProperties.UserId = context.User?.Identity?.Name;
            contextProperties.UserRole = context.User?.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
            contextProperties.UserLogin = context.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
            ContextContainer.Context = contextProperties;
            await this._next(context);
        }
    }
}