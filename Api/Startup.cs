using Api.Middleware;
using Api.Scheduler;
using Api.Utils.AutoMapper;
using AutoMapper;
using Domain.Audit;
using Domain.Authenticate;
using Domain.Code;
using Domain.FileStorage;
using Domain.Push;
using Domain.Srbac;
using Domain.Token;
using Domain.User;
using FirebaseCoreSDK;
using FirebaseCoreSDK.Configuration;
using FirebaseCoreSDK.Firebase.Auth.ServiceAccounts;
using Infrastructure;
using Infrastructure.AppSettings;
using Infrastructure.Code;
using Infrastructure.Contexts;
using Infrastructure.Crypto;
using Infrastructure.Email;
using Infrastructure.FileStorage;
using Infrastructure.Host;
using Infrastructure.Push;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Audit;
using Infrastructure.Repositories.Code;
using Infrastructure.Repositories.File;
using Infrastructure.Repositories.Srbac;
using Infrastructure.Repositories.Token;
using Infrastructure.Repositories.User;
using Infrastructure.SMS;
using Infrastructure.Template;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Implementations;
using System;
using System.IO;
using System.Text;

namespace Api
{
    public class Startup
    {
        private static readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        private static readonly string AppSettings =
            string.IsNullOrEmpty(Env) ? "appsettings.json" : $"appsettings.{Env}.json";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(options =>
            {
                options.UseNpgsql(
                        Configuration.GetConnectionString("DefaultConnection"),
                        x => x.MigrationsAssembly("DBMigrations")
                )
                    // .UseLoggerFactory(LoggerFactory.Create(p => p.AddConsole()))
                ;
            });

            #region Firebase

            // ToDo: Edit config and activate this
            /* var configuration =
                 new FirebaseSDKConfiguration
                 {
                     Credentials =
                         new JsonServiceAccountCredentials(
                             Path.Combine(Directory.GetCurrentDirectory(), "kpd-firebase.json")
                         )
                 };
             var firebaseClient = new FirebaseClient(configuration);
             services.AddSingleton(firebaseClient);
            */
            #endregion

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #region DI Service

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile(AppSettings, optional: true, reloadOnChange: false);
            services.AddSingleton(builder.Build());
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ICodeService, CodeService>();
            services.AddTransient<IUserService, UserService>();

            services.AddSingleton<ISrbacService, SrbacService>();
            services.AddSingleton<CryptoHelper>();
            services.AddSingleton<IHostedService, ScheduleTask>();
            // ToDo: Edit config and activate this
            // services.AddTransient<IPushService, NotificationService>();

            var fileStorageConfig = Configuration
                .GetSection(nameof(FileStorageConfiguration))
                .Get<FileStorageConfiguration>();
            services.AddSingleton(fileStorageConfig);
            services.AddTransient<IFileStorageService, FileStorageService>();
            services.AddTransient<IAuditService, AuditService>();

            var appSettingsConfig = Configuration
                .GetSection(nameof(AppSettingsConfiguration))
                .Get<AppSettingsConfiguration>();
            services.AddSingleton(appSettingsConfig);
            #endregion

            #region DI Repository

            services.AddSingleton(sp =>
            {
                using var scope = sp.CreateScope();
                var dbContext = scope.ServiceProvider.GetService<Context>();
                var settings = scope.ServiceProvider.GetService<AppSettingsConfiguration>();
                return new SrbacRepository(dbContext, settings);
            });
            services.AddTransient<AuditRepository>();
            services.AddTransient<FileRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<TokenRepository>();
            services.AddTransient<CodeRepository>();
            services.AddTransient<SqlRepository>();
            #endregion

            #region DI Infrastructure

            services.AddSingleton(typeof(TemplateContainer));

            var emailConfig = Configuration
                .GetSection(nameof(EmailConfiguration))
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddSingleton<IEmailSender, EmailSender>();

            var smsConfig = Configuration
                .GetSection(nameof(SmsConfiguration))
                .Get<SmsConfiguration>();
            services.AddSingleton(smsConfig);
            if (smsConfig.IsStub)
                services.AddSingleton<ISMSSender, StubSMSSender>();
            // ToDo: Edit config and activate this
            // services.AddSingleton<IPushSender, FirebasePushSender>();

            var hostConfig = Configuration
                .GetSection(nameof(HostConfiguration))
                .Get<HostConfiguration>();
            services.AddSingleton(hostConfig);
            
            var codeConfig = Configuration
                .GetSection(nameof(CodeConfiguration))
                .Get<CodeConfiguration>();
            services.AddSingleton(codeConfig);

            services.AddTransient<InitializeInfrastructure>();

            #endregion

            services.AddCors();
            services.AddMvc(p => p.EnableEndpointRouting = false);

            var key = Encoding.ASCII.GetBytes(Configuration["AppSettings:Secret"]);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = false
                    };
                });

            var securityScheme = new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                BearerFormat = "Bearer {authToken}",
                Description = "JWT Token",
                Type = SecuritySchemeType.ApiKey
            };
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "API Template", Version = "v1"});

                // c.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "Api.xml"));
                // c.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "Domain.xml"));
                
                c.AddSecurityDefinition(
                    "Bearer", securityScheme
                );
                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            InitializeInfrastructure infrastructure
        )
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "KPD v1"); });
            
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            
            // if (env.IsDevelopment())
            // {
            app.UseDeveloperExceptionPage();
            // }

            app.UseAuthentication();

            app.UseMiddleware<TokenMiddleware>();

            app.UseMvc();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            UpdateDatabase(app);

            infrastructure.FileStorage();
            
            var logger = loggerFactory.CreateLogger("LoggerInStartup");
            logger.LogInformation($"\n\n{DateTime.Now} | Startup logger was launched");
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<Context>();

            context.Database.Migrate();
        }
    }
}