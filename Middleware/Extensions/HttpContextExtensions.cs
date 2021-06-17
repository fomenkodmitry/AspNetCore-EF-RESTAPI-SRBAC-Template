using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Middleware.Options;

namespace Middleware.Extensions
{
    public static class HttpContextExtensions
    {
        internal static bool DoNotLogCurrentRequest(
            this HttpContext context,
            LogRequestResponseOptions options)
        {
            return options.ExcludeRequestPaths != null && options.ExcludeRequestPaths.Any<string>((Func<string, bool>) (exclude => context.Request.Path.Value.Contains(exclude)));
        }
    }
}