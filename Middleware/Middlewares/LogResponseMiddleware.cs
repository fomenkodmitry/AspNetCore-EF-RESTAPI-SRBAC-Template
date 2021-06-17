using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Middleware.Extensions;
using Middleware.Options;

namespace Middleware.Middlewares
{
    public class LogResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogResponseMiddleware> _logger;
        private readonly LogRequestResponseOptions _options;

        public LogResponseMiddleware(
            RequestDelegate next,
            ILogger<LogResponseMiddleware> logger,
            IOptions<LogRequestResponseOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!_options.LogResponse || context.DoNotLogCurrentRequest(this._options))
            {
                await _next(context);
            }
            else
            {
                Stream bodyStream = context.Response.Body;
                using (MemoryStream responseBodyStream = new MemoryStream())
                {
                    context.Response.Body = responseBodyStream;
                    await _next(context);
                    responseBodyStream.Seek(0L, SeekOrigin.Begin);
                    using (StreamReader streamReader = new StreamReader((Stream) responseBodyStream))
                    {
                        string end = await streamReader.ReadToEndAsync();
                        _logger.LogInformation(string.Format("RESPONSE HTTPSTATUS: {0}, BODY: {1}", (object) context.Response.StatusCode, (object) end));
                        responseBodyStream.Seek(0L, SeekOrigin.Begin);
                        await responseBodyStream.CopyToAsync(bodyStream);
                    }
                }
            }
        }
    }
}