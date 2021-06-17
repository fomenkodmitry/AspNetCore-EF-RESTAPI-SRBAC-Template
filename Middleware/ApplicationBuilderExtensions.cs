using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Middleware.Middlewares;
using Middleware.Options;

namespace Middleware
{
  public static class ApplicationBuilderExtensions
  {

    public static IApplicationBuilder UseRequestResponseLogging(
      this IApplicationBuilder app)
    {
      if (app == null)
        throw new ArgumentNullException(nameof (app));
      app.UseMiddleware<LogRequestMiddleware>();
      app.UseMiddleware<LogResponseMiddleware>();
      return app;
    }

    public static IApplicationBuilder UseTraceRequestLogging(
      this IApplicationBuilder app)
    {
      return app != null ? app.UseMiddleware<TraceRequestMiddleware>() : throw new ArgumentNullException(nameof (app));
    }

    public static IApplicationBuilder UseVersionMiddleware(
      this IApplicationBuilder app)
    {
      return app != null ? app.UseMiddleware<VersionMiddleware>() : throw new ArgumentNullException(nameof (app));
    }

    public static IApplicationBuilder UseCorrelationId(
      this IApplicationBuilder app)
    {
      return app != null ? app.UseMiddleware<CorrelationIdMiddleware>() : throw new ArgumentNullException(nameof (app));
    }

    public static IApplicationBuilder UseLocalization(
      this IApplicationBuilder app)
    {
      return app != null ? app.UseMiddleware<LocalizationMiddleware>() : throw new ArgumentNullException(nameof (app));
    }

    public static IApplicationBuilder UseCorrelationId(
      this IApplicationBuilder app,
      CorrelationIdOptions options)
    {
      if (app == null)
        throw new ArgumentNullException(nameof (app));
      return options != null ? app.UseMiddleware<CorrelationIdMiddleware>((object) Microsoft.Extensions.Options.Options.Create<CorrelationIdOptions>(options)) : throw new ArgumentNullException(nameof (options));
    }

  }
}