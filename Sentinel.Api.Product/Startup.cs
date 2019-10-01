using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Sentinel.Common;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Mercan.HealthChecks.Common;
using Mercan.HealthChecks.Mongo;
using Mercan.HealthChecks.Redis;
using System.Net.Http.Headers;
using System.Net.Http;
using Mercan.HealthChecks.RabbitMQ;
using EasyNetQ;
using Mercan.HealthChecks.Common.CheckCaller;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;
using Sentinel.Repos.Repositories;
using CorrelationId;
using Sentinel.Model.Product.Dto;
using Sentinel.Model.Product;

namespace Sentinel.Api.Product
{
    public class Startup
    {
        private bool InDocker { get { return Environment.EnvironmentName == "DOTNET_RUNNING_IN_CONTAINER" ? true : false; } }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            this.Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IServiceCollection>(services);
            services.AddSingleton<IConfiguration>(Configuration);

            var healthcheckBuilder = services.AddHealthChecks()
              .AddProcessList()
              .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory")
              .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "AvailableMBytes")
              .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "PercentCommittedBytesInUse", "PercentCommittedBytesInUse_Base")
              .AddSystemInfoCheck()
             .AddPrivateMemorySizeCheckMB(10)
             .AddWorkingSetCheckKB(500000)
             //.AddCheck<SlowDependencyHealthCheck>("Slow", failureStatus: null, tags: new[] { "ready", })
             .SqlConnectionHealthCheck(Configuration["SentinelConnection"])
             // .AddApiIsAlive(Configuration.GetSection("sentinel-ui-sts:ClientOptions"), "health/isalive")
             // .AddApiIsAlive(Configuration.GetSection("sentinel-api-member:ClientOptions"), "health/isalive")
             // .AddApiIsAlive(Configuration.GetSection("sentinel-api-product:ClientOptions"), "health/isalive")
             // .AddApiIsAlive(Configuration.GetSection("sentinel-api-comms:ClientOptions"), "health/isalive")
             .AddMongoHealthCheck(Configuration["Mongodb:ConnectionString"])
             // .AddRabbitMQHealthCheck(Configuration["RabbitMQConnection"])
             .AddRedisHealthCheck(Configuration["RedisConnection"])
             .AddDIHealthCheck(services);

            if (Environment.EnvironmentName != "dockertest")
            {
                healthcheckBuilder.AddRabbitMQHealthCheckWithDiIBus();
            }

            // services.AddMiniProfiler(options =>
            //     options.RouteBasePath = "/profiler"
            // );

            services.AddApplicationInsightsTelemetry("15ce6ddc-8d32-418e-9d5c-eed1cd7d6096");
            services.AddApplicationInsightsKubernetesEnricher();

            services.ConfigureJwtAuthService(Configuration);
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains();
                //.AllowCredentials();
            }));

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.IncludeXmlComments(XmlCommentsFilePath);

                options.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "Bearer "
                });

            });
            services.AddSingleton<EasyNetQ.IBus>((ctx) =>
            {
                return RabbitHutch.CreateBus(Configuration["RabbitMQConnection"]);
            });
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["RedisConnection"];
                options.InstanceName = "ApiProduct";
            });

            // services.AddHttpClient("run_with_try", options =>
            // {
            //     options.Timeout = new TimeSpan(0, 2, 0);
            //     options.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            //     options.DefaultRequestHeaders.Add("OData-Version", "4.0");
            //     options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // }).ConfigurePrimaryHttpMessageHandler<CertMessageHandler>()
            // // ConfigurePrimaryHttpMessageHandler((ch) =>
            // // {
            // //     var handler = new HttpClientHandler();
            // //     handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            // //     handler.ClientCertificates.Add(HttpClientHelpers.GetCert());
            // //     return handler;

            // // })
            // // .AddHttpMessageHandler()
            // // .AddHttpMessageHandler<OAuthTokenHandler>()
            // //.AddHttpMessageHandler(*)
            // .AddPolicyHandler(HttpClientHelpers.GetRetryPolicy())
            // .AddPolicyHandler(HttpClientHelpers.GetCircuitBreakerPolicy());

            services.AddHttpClient<HealthCheckReportDownloaderService>("HealthCheckReportDownloader", options =>
            {
                // options.BaseAddress = new Uri(Configuration["CrmConnection:ServiceUrl"] + "api/data/v8.2/");
                options.Timeout = new TimeSpan(0, 2, 0);
                options.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                options.DefaultRequestHeaders.Add("OData-Version", "4.0");
                options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            })
            // .ConfigurePrimaryHttpMessageHandler((ch) =>
            // {
            //     var handler = new HttpClientHandler();
            //     handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            //     handler.ClientCertificates.Add(HttpClientHelpers.GetCert());
            //     return handler;
            // })
            //.AddHttpMessageHandler()
            // .AddHttpMessageHandler<OAuthTokenHandler>()
            .AddPolicyHandler(HttpClientHelpers.GetRetryPolicy())
            .AddPolicyHandler(HttpClientHelpers.GetCircuitBreakerPolicy());


            services.AddMangoRepo<ProductInfoDtoV2>(Configuration.GetSection("Mongodb"));
            services.AddMangoRepo<VendorInfo>(Configuration.GetSection("Mongodb"), "vendor");
            services.AddMangoRepo<CategoryInfo>(Configuration.GetSection("Mongodb"), "category");
            services.AddMangoRepo<TechnologyInfo>(Configuration.GetSection("Mongodb"), "technology");
            // services.AddScoped<ProductRepo>();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddCorrelationId();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider, IHostApplicationLifetime lifetime, IDistributedCache cache)
        {
            lifetime.ApplicationStarted.Register(() =>
                      {
                          var currentTimeUTC = DateTime.UtcNow.ToString();
                          byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
                          var options = new DistributedCacheEntryOptions()
                              .SetSlidingExpiration(TimeSpan.FromSeconds(20));
                          cache.Set("cachedTimeUTC", encodedCurrentTimeUTC, options);
                      });

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Enviroment", Environment.EnvironmentName)
            .Enrich.WithProperty("ApplicationName", "Api App")
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            .WriteTo.File("Logs/logs.txt");
            //.WriteTo.Elasticsearch()
            logger.WriteTo.Console();
            loggerFactory.AddSerilog();
            Log.Logger = logger.CreateLogger();
            app.UseExceptionLogger();

            app.UseSwagger(e => { e.AddHealthCheckSwaggerOptions(); });
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = string.Empty;
            });

            app.UseCors("MyPolicy");
            app.UseRouting();
            app.UseAllAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // app.UseMiniProfiler();
            app.UseHealthChecksWithAuth("/Health/IsAliveAndWell", new HealthCheckOptions()
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






            app.UseRequestResponseLogger();
            app.UseCorrelationId();
        }

        static string XmlCommentsFilePath
        {
            get
            {
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var basePath = AppContext.BaseDirectory;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
