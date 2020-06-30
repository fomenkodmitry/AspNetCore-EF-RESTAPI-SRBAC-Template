using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Api
{
    public class Program
    {
        private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static readonly string LogFilePath = string.IsNullOrEmpty(Env) ? "nlog.config" : $"nlog.{Env}.config";
        
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog(LogFilePath).GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(
                    logging =>
                    {
                        logging.AddConsole();
                        logging.AddDebug();
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Debug);
                    })
                .UseNLog();
    }
}