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
using SSentinel.Handler.Comms.HostedServices;

namespace Sentinel.Handler.Comms
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
            services.AddHostedService<HealthCheckSubscribeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Enviroment", env.EnvironmentName)
            .Enrich.WithProperty("ApplicationName", "Api App")
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            .WriteTo.File("Logs/logs.txt");
            //.WriteTo.Elasticsearch()

            logger.WriteTo.Console();
            loggerFactory.AddSerilog();
            Log.Logger = logger.CreateLogger();
            app.UseExceptionLogger();

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
