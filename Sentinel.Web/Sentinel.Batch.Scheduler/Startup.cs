using System;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Mercan.HealthChecks.Common.Checks;
using Mercan.HealthChecks.Common;
using EasyNetQ;
using Mercan.Common.ScheduledTask;
using Sentinel.Batch.Scheduler.ScheduledTasks;

namespace Sentinel.Batch.Scheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureJwtAuthService(IServiceCollection services)
        {
            var audienceConfig = Configuration.GetSection("Tokens");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],

                // Validate the token expiry
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer("azure", cfg =>
          {
              cfg.RequireHttpsMetadata = false;
              cfg.SaveToken = true;
              cfg.Authority = Configuration["AzureAd:Instance"] + "/" + Configuration["AzureAD:TenantId"];
              cfg.Audience = Configuration["AzureAd:ClientId"];
          })
            .AddJwtBearer("sts", cfg =>
             {
                 cfg.TokenValidationParameters = tokenValidationParameters;
             });
            //use both jwt schemas interchangeably  https://stackoverflow.com/questions/49694383/use-multiple-jwt-bearer-authentication
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().AddAuthenticationSchemes("azure", "sts").Build();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHealthChecks()
            // .AddProcessList()
            // .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory")
            // .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "AvailableMBytes")
            // .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "PercentCommittedBytesInUse", "PercentCommittedBytesInUse_Base")
             .AddSystemInfoCheck()
            //.AddPrivateMemorySizeCheckMB(1000)
            .AddWorkingSetCheckKB(250000);
            // .AddCheck<SlowDependencyHealthCheck>("Slow", failureStatus: null, tags: new[] { "ready", })
            // .SqlConnectionHealthCheck(Configuration["SentinelConnection"])
            // .AddApiIsAlive(Configuration.GetSection("sentinel-ui-sts:ClientOptions"), "health/isalive")
            // .AddApiIsAlive(Configuration.GetSection("sentinel-api-member:ClientOptions"), "health/isalive")
            // .AddApiIsAlive(Configuration.GetSection("sentinel-api-product:ClientOptions"), "health/isalive")
            // .AddApiIsAlive(Configuration.GetSection("sentinel-api-comms:ClientOptions"), "health/isalive")
            // .AddMongoHealthCheck(Configuration["Mongodb:ConnectionString"])
            // .AddRabbitMQHealthCheck(Configuration["RabbitMQConnection"])
            // .AddRedisHealthCheck(Configuration["RedisConnection"])
            // .AddDIHealthCheck(services);
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //services.AddSignalR();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowCredentials();
            }));

            services.AddSingleton<EasyNetQ.IBus>((ctx) =>
            {
                return RabbitHutch.CreateBus(Configuration["RabbitMQConnection"]);
            });
            // services.AddHostedService<HealthCheckSubscribeService>();

            services.AddSingleton<IConfiguration>(Configuration);

            // Scheduled Tasks Added             
            services.AddSingleton<IScheduledTask, WorkDayEndsTask>();
            services.AddSingleton<IScheduledTask, WorkDayStartsTask>();
            services.AddSingleton<IScheduledTask, HealthCheckRequestTask>();

            // Scheduled Tasks Runner             
            services.AddScheduler();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
            }
            else
            {
            }

            app.UseStaticFiles();
            var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Enviroment", env.EnvironmentName)
            .Enrich.WithProperty("ApplicationName", "Api App")
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            .WriteTo.File("Logs/logs.txt");

            logger.WriteTo.Console();
            loggerFactory.AddSerilog();
            Log.Logger = logger.CreateLogger();
            app.UseExceptionLogger();
            app.UseDefaultFiles();
            app.UseCors("MyPolicy");

            app.UseHealthChecks("/Health/IsAliveAndWell", new HealthCheckOptions()
            {
                ResponseWriter = WriteResponses.WriteListResponse,
            });

            app.Map("/Health/IsAlive", (ap) =>
            {
                ap.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"IsAlive\":true}");
                });
            });

        }
    }
}
