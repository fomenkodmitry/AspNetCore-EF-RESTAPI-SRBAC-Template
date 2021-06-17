using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middleware.Middlewares
{
    public class VersionMiddleware
    {
        private static string AssemblyVersion;
        private readonly RequestDelegate _next;
        private readonly string _apiPath = "/version";

        public VersionMiddleware(RequestDelegate next)
        {
            _next = next;
            AssemblyVersion = GetVersion();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get)
            {
                if (context.Request.Path.Value.TrimEnd('/').Equals(this._apiPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(AssemblyVersion);
                }
                else
                    await _next(context);
            }
            else
                await _next(context);
        }

        private static string GetVersion() => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion;
    }
}