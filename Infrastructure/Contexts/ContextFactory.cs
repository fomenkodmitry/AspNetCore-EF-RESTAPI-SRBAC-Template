using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

// dotnet ef --project ../DBMigrations migrations add MigrateName
namespace Infrastructure.Contexts
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        private static readonly string AppSettings =
            string.IsNullOrEmpty(Env) ? "appsettings.json" : $"appsettings.{Env}.json";

        public Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + "/../Api")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(AppSettings, optional: true)
                .Build();

            optionsBuilder.UseNpgsql(
                config.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("DBMigrations")
            );

            return new Context(optionsBuilder.Options);
        }
    }
}