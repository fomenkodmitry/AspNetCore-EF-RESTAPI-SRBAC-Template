using System.Threading.Tasks;
using Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Middleware.Options;

namespace Middleware.Middlewares
{
    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LocalizationMiddlewareOptions _options;

        public LocalizationMiddleware(
            RequestDelegate next,
            IOptions<LocalizationMiddlewareOptions> config)
        {
            _next = next;
            _options = config.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(_options.LanguageHeaderName, out var stringValues) && !string.IsNullOrEmpty((string) stringValues))
                LocalizationManager.SetLanguage(stringValues);
            else
                LocalizationManager.SetLanguage(LocalizationManager.DefaultLanguage);
            await this._next(context);
        }
    }
}