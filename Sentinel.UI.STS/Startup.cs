using System;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Sentinel.UI.STS.Models;
using Sentinel.UI.STS.DbContexts;
using Sentinel.UI.STS.Repositories;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sentinel.UI.STS.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Mercan.HealthChecks.Common.Checks;

namespace Sentinel.UI.STS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")))
           .AddDbContext<TokenDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

            services.Configure<TokenSettings>(Configuration.GetSection("Tokens"));
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("azure", cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.Authority = Configuration["AzureAd:Instance"] + "/" + Configuration["AzureAD:TenantId"];
                cfg.Audience = Configuration["AzureAd:ClientId"];
            })
            .AddJwtBearer("local", cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Tokens:Secret"])),
                    RoleClaimType = ClaimTypes.Role
                };
            });

            //use both jwt schemas interchangeably  https://stackoverflow.com/questions/49694383/use-multiple-jwt-bearer-authentication
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().AddAuthenticationSchemes("azure", "local").Build();
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

            // services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
            services.AddVersionedApiExplorer(options =>
           {
               options.GroupNameFormat = "'v'VVV";
               options.SubstituteApiVersionInUrl = true;
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
                        Title = $"Sentinel.UI.STS API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString()
                    });
                }
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddMailService(Configuration.GetSection("SMTP"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
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
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
            app.UseCors("MyPolicy");
            app.UseCookiePolicy();

            app.UseAuthentication();

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
