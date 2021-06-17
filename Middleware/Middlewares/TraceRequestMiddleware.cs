using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace Middleware.Middlewares
{
    public class TraceRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogRequestMiddleware> _logger;

        public TraceRequestMiddleware(RequestDelegate next, ILogger<LogRequestMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string displayUrl = context.Request.GetDisplayUrl();
            this._logger.LogInformation(context.Request.Method + " " + displayUrl);
            await this._next(context);
        }
    }
}