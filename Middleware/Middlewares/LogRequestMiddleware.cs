using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Middleware.Extensions;
using Middleware.Options;

namespace Middleware.Middlewares
{
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogRequestMiddleware> _logger;
        private readonly LogRequestResponseOptions _options;

        public LogRequestMiddleware(
            RequestDelegate next,
            ILogger<LogRequestMiddleware> logger,
            IOptions<LogRequestResponseOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!_options.LogRequest || context.DoNotLogCurrentRequest(_options))
            {
                await _next(context);
            }
            else
            {
                using (MemoryStream requestBodyStream = new MemoryStream())
                {
                    using (Stream originalRequestBody = context.Request.Body)
                    {
                        await context.Request.Body.CopyToAsync((Stream) requestBodyStream);
                        requestBodyStream.Seek(0L, SeekOrigin.Begin);
                        var displayUrl = context.Request.GetDisplayUrl();
                        var end = new StreamReader((Stream) requestBodyStream).ReadToEnd();
                        _logger.LogInformation(
                            $"METHOD: {context.Request.Method}, REQUEST URL: {displayUrl}, BODY: {end}");
                        requestBodyStream.Seek(0L, SeekOrigin.Begin);
                        context.Request.Body = requestBodyStream;
                        await _next(context);
                        context.Request.Body = originalRequestBody;
                    }
                }
            }
        }
    }
}