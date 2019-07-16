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
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Mercan.HealthChecks.Common.Checks;
using Mercan.HealthChecks.Mongo;
using Mercan.HealthChecks.RabbitMQ;
using Mercan.HealthChecks.Redis;

using Microsoft.EntityFrameworkCore;
using Sentinel.Repos.Sql;
using Sentinel.Repos.Repositories;
using Serilog.Sinks.Elasticsearch;
using CorrelationId;
using Serilog.Formatting.Elasticsearch;
using EasyNetQ;
using Sentinel.Model.Product.Dto;
using Sentinel.Model.Product;
using Mercan.HealthChecks.Common;

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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //  services.AddDbContext<SentinelDbContext>(o => { o.UseInMemoryDatabase(databaseName: "memorydb"); });
            services.AddMiniProfiler(options => options.RouteBasePath = "/profiler");
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
            services.AddMangoRepo<ProductInfoDtoV2>(Configuration.GetSection("Mongodb"));
            services.AddMangoRepo<VendorInfo>(Configuration.GetSection("Mongodb"), "vendor");
            services.AddMangoRepo<CategoryInfo>(Configuration.GetSection("Mongodb"), "category");
            services.AddMangoRepo<TechnologyInfo>(Configuration.GetSection("Mongodb"), "technology");

            //  services.TryAdd(new ServiceDescriptor(typeof(MangoBaseRepo<ProductInfoDtoV2>), typeof(MangoBaseRepo<ProductInfoDtoV2>), ServiceLifetime.Scoped));


            services.AddSingleton<EasyNetQ.IBus>((ctx) =>
            {
                return RabbitHutch.CreateBus(Configuration["RabbitMQConnection"]);
            });
            services.AddScoped<ProductRepo>();
            // services.AddScoped<TriggerHandler>();
            // services.AddTriggerHandler<TriggerHandler>();

            services.AddHealthChecks()
             .AddProcessList()
             .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory")
             .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "AvailableMBytes")
             .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "PercentCommittedBytesInUse", "PercentCommittedBytesInUse_Base")
             .AddSystemInfoCheck()
            .AddPrivateMemorySizeCheckMB(10)
            .AddWorkingSetCheckKB(500000)
            //.AddCheck<SlowDependencyHealthCheck>("Slow", failureStatus: null, tags: new[] { "ready", })
            .SqlConnectionHealthCheck(Configuration["SentinelConnection"])
            .AddApiIsAlive(Configuration.GetSection("sentinel-ui-sts:ClientOptions"), "health/isalive")
            .AddApiIsAlive(Configuration.GetSection("sentinel-api-member:ClientOptions"), "health/isalive")
            .AddApiIsAlive(Configuration.GetSection("sentinel-api-product:ClientOptions"), "health/isalive")
            .AddApiIsAlive(Configuration.GetSection("sentinel-api-comms:ClientOptions"), "health/isalive")
            .AddMongoHealthCheck(Configuration["Mongodb:ConnectionString"])
            .AddRabbitMQHealthCheck(Configuration["RabbitMQConnection"])
            .AddRedisHealthCheck(Configuration["RedisConnection"])
            .AddDIHealthCheck(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            ConfigureJwtAuthService(services);
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
                o.DefaultApiVersion = new ApiVersion(2, 0);
                o.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddCorrelationId();
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

            // var mongoClient = new MongoClient(Configuration["Mongodb:ConnectionString"]);
            // var MongoDblogs = mongoClient.GetDatabase("logs");

            var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Enviroment", env.EnvironmentName)
            .Enrich.WithProperty("ApplicationName", "Api App")
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            //  .WriteTo.MongoDBCapped(MongoDblogs, collectionName: "rollingapplog")
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
                            Produces = new string[] { "application/json" },
                            Responses = new Dictionary<string, Response>{
                                {"200",new Response{Description="Success"}},
                                {"503",new Response{Description="Failed"}}
                            }
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
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            app.UseCors("MyPolicy");
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseCorrelationId();
            app.UseMiniProfiler();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHealthChecks("/Health/IsAliveAndWell", new HealthCheckOptions()
            {
                // This custom writer formats the detailed status as JSON.
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
