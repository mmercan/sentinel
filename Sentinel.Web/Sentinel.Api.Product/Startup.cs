using System;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
// using Sentinel.Api.Product.Models;
// using Sentinel.Api.Product.DbContexts;
// using Sentinel.Api.Product.Repositories;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Versioning;
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

using Microsoft.EntityFrameworkCore;
using Sentinel.Repos.Sql;
using Sentinel.Repos.Repositories;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Caching.Distributed;
// using Mercan.Common.Mongo;
using Mercan.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Serilog.Sinks.Elasticsearch;
using CorrelationId;
using Sentinel.Model.Product.Dto;
using Serilog.Formatting.Elasticsearch;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Mercan.HealthChecks.Common.Checks;
using EasyNetQ;

namespace Sentinel.Api.Product
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private bool InDocker { get { return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"; } }
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
            services.AddSingleton<IServiceCollection>(services);
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddDbContext<SentinelDbContext>(o =>
            {
                if (InDocker)
                {
                    o.UseInMemoryDatabase(databaseName: "Add_writes_to_database");
                }
                else
                {
                    o.UseInMemoryDatabase(databaseName: "Add_writes_to_database");
                }
            });

            services.AddMiniProfiler(options =>
                options.RouteBasePath = "/profiler"
            );

            if (InDocker)
            {

                var redisconn = Configuration["RedisConnection"];
                services.AddDistributedRedisCache(options =>
               {
                   options.Configuration = redisconn;
                   options.InstanceName = "productapi";
               });
            }

            // services.Configure<MangoBaseRepoSettings>(Configuration.GetSection("Mongodb"));
            //  services.AddMangoRepo<ProductInfoDtoV2>(Configuration.GetSection("Mongodb"));
            // services.TryAdd(new ServiceDescriptor(typeof(MangoBaseRepo<ProductInfoDtoV2>), typeof(MangoBaseRepo<ProductInfoDtoV2>), ServiceLifetime.Scoped));




            services.AddSingleton<IBus>(RabbitHutch.CreateBus(Configuration["RabbitMQConnection"]));
            services.AddScoped<ProductRepo>();
            // services.AddScoped<TriggerHandler>();
            services.AddTriggerHandler<TriggerHandler>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHealthChecks()
            .AddProcessList()
            .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory")
            .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "AvailableMBytes")
            .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "PercentCommittedBytesInUse", "PercentCommittedBytesInUse_Base")
            .AddSystemInfoCheck();
            // .AddWorkingSetCheck(10000000)
            // //.AddCheck<SlowDependencyHealthCheck>("Slow", failureStatus: null, tags: new[] { "ready", })
            // .SqlConnectionHealthCheck(Configuration["SentinelConnection"])
            // .AddApiIsAlive(Configuration.GetSection("sentinel-ui-sts:ClientOptions"), "api/healthcheck/isalive")
            // .AddApiIsAlive(Configuration.GetSection("sentinel-api-member:ClientOptions"), "api/healthcheck/isalive")
            // .AddApiIsAlive(Configuration.GetSection("sentinel-api-product:ClientOptions"), "api/healthcheck/isalive")
            // .AddApiIsAlive(Configuration.GetSection("sentinel-api-comms:ClientOptions"), "api/healthcheck/isalive")
            // .AddMongoHealthCheck(Configuration["Mongodb:ConnectionString"])
            // .AddRabbitMQHealthCheck(Configuration["RabbitMQConnection"])
            // .AddRedisHealthCheck(Configuration["RedisConnection"])
            // .AddDIHealthCheck(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
            services.AddAuthentication();
            // .AddAzureAD(options => Configuration.Bind("AzureAd", options));
            ConfigureJwtAuthService(services);

            services.AddAutoMapper();

            services.AddMvc(options =>
            {
                // var policy = new AuthorizationPolicyBuilder()
                //     .RequireAuthenticatedUser()
                //     .Build();
                // options.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCorrelationId();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowCredentials();
            }));

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                    description.GroupName,
                    new Info()
                    {
                        Title = $"Sentinel.Api.Product API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString()
                    });
                }
                // Set the comments path for the Swagger JSON and UI.
                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // options.IncludeXmlComments(xmlPath);
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseRequestResponseLogger();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var mongoClient = new MongoClient(Configuration["Mongodb:ConnectionString"]);
            var MongoDblogs = mongoClient.GetDatabase("logs");

            var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Enviroment", env.EnvironmentName)
            .Enrich.WithProperty("ApplicationName", "Api App")
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            .WriteTo.MongoDBCapped(MongoDblogs, collectionName: "rollingapplog")
            .WriteTo.File("Logs/logs.txt")
            //.WriteTo.Kafka(new KafkaSinkOptions(topic: "test", brokers: new[] { new Uri(Configuration["KafkaUrl"]) }))
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://sentinel-db-elasticsearch:9200"))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                TemplateName = "productslog",
                IndexFormat = "productslog-{0:yyyy.MM.dd}",
                InlineFields = true,
                // IndexDecider = (@event, offset) => "test_elapsedtimes",
                CustomFormatter = new ElasticsearchJsonFormatter()
            });

            logger.WriteTo.Console();
            loggerFactory.AddSerilog();
            Log.Logger = logger.CreateLogger();
            app.UseExceptionLogger();
            // move  UseDefaultFiles to first line
            // app.UseFileServer();
            app.UseDefaultFiles();

            app.UseSwagger(e =>
            {
                e.PreSerializeFilters.Add((doc, req) =>
                {
                    doc.Paths.Add("/Health/IsAliveAndWell", new PathItem
                    {
                        Get = new Operation
                        {
                            Tags = new List<string> { "HealthCheck" },
                            Produces = new string[] { "application/json" }
                        }
                    });

                    doc.Paths.Add("/Health/IsAlive", new PathItem
                    {
                        Get = new Operation
                        {
                            Tags = new List<string> { "HealthCheck" },
                            Produces = new string[] { "application/json" }
                        }
                    });
                });
            });

            app.UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
            app.UseCors("MyPolicy");
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseCorrelationId();

            app.UseMiniProfiler();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHealthChecks("/Health/IsAliveAndWell", new HealthCheckOptions()
            {
                // This custom writer formats the detailed status as JSON.
                ResponseWriter = WriteResponse,
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

        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            //As Dictionary
            // var json = new JObject(
            //     new JProperty("status", result.Status.ToString()),
            //     new JProperty("duration", result.TotalDuration),
            //     new JProperty("results", new JObject(result.Entries.Select(pair =>
            //         new JProperty(pair.Key, new JObject(
            //             new JProperty("status", pair.Value.Status.ToString()),
            //             new JProperty("description", pair.Value.Description),
            //             new JProperty("duration", pair.Value.Duration),

            //             new JProperty("data", new JObject(pair.Value.Data.Select(p => new JProperty(p.Key, p.Value)))),
            //             new JProperty("exception", pair.Value.Exception?.Message) //new JObject(pair.Value.Exception.Select(p => new JProperty(p.Key, p.Value))))
            //                                         ))))));
            // return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));


            //As Array
            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("duration", result.TotalDuration),
                new JProperty("results", new JArray(result.Entries.Select(pair =>
                  new JObject(
                       new JProperty("name", pair.Key.ToString()),
                       new JProperty("status", pair.Value.Status.ToString()),
                       new JProperty("description", pair.Value.Description),
                       new JProperty("duration", pair.Value.Duration),
                       new JProperty("type", pair.Value.Data.FirstOrDefault(p => p.Key == "type").Value),
                       new JProperty("data", new JObject(pair.Value.Data.Select(p => new JProperty(p.Key, p.Value)))),
                       new JProperty("exception", pair.Value.Exception?.Message)
                 )
                 ))));
            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }
    }
}
