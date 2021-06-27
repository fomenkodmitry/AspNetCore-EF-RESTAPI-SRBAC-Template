using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Middleware.Options;

namespace Middleware.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CorrelationIdOptions _options;

        public CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof (options));
            _next = next ?? throw new ArgumentNullException(nameof (next));
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(CorrelationIdOptions.DefaultHeader, out var correlationId))
            {
                correlationId = (StringValues) _options.GenerateValueFunc();
                context.Request.Headers.Add(CorrelationIdOptions.DefaultHeader, correlationId);
            }
            if (_options.IncludeInResponse)
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add(CorrelationIdOptions.DefaultHeader, correlationId);
                    return Task.CompletedTask;
                });
            await this._next(context);
        }
    }
}