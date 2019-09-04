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
using Mercan.HealthChecks.Common;
using Sentinel.Api.HealthMonitoring.HostedServices;
using EasyNetQ;
using Mercan.HealthChecks.Common.CheckCaller;
using System.Net.Http.Headers;
using Polly;
using System.Net.Http;
using Polly.Extensions.Http;
using System.Security.Cryptography.X509Certificates;

namespace Sentinel.Api.HealthMonitoring
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment env { get; }

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
            // use both jwt schemas interchangeably  https://stackoverflow.com/questions/49694383/use-multiple-jwt-bearer-authentication
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

            var healthcheckBuilder = services.AddHealthChecks()
               .AddProcessList()
               .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory")
               .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "AvailableMBytes")
               .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "PercentCommittedBytesInUse", "PercentCommittedBytesInUse_Base")
               .AddSystemInfoCheck()
              .AddWorkingSetCheckKB(450000)
              //.AddCheck<SlowDependencyHealthCheck>("Slow", failureStatus: null, tags: new[] { "ready", })
              .SqlConnectionHealthCheck(Configuration["SentinelConnection"])
              // .AddApiIsAlive(Configuration.GetSection("sentinel-ui-sts:ClientOptions"), "health/isalive")
              .AddApiIsAlive(Configuration.GetSection("sentinel-api-member:ClientOptions"), "health/isalive")
              .AddApiIsAlive(Configuration.GetSection("sentinel-api-product:ClientOptions"), "health/isalive")
              .AddApiIsAlive(Configuration.GetSection("sentinel-api-comms:ClientOptions"), "health/isalive")
              .AddMongoHealthCheck(Configuration["Mongodb:ConnectionString"])
              //.AddRabbitMQHealthCheck(Configuration["RabbitMQConnection"])
              .AddRedisHealthCheck(Configuration["RedisConnection"])
              .AddDIHealthCheck(services);

            if (env.EnvironmentName != "dockertest")
            {
                healthcheckBuilder.AddRabbitMQHealthCheckWithDiIBus();
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
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
                        Title = $"Sentinel.Api.HealthMonitoring API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString()
                    });
                }
                options.AddSecurityDefinition("BearerAuth", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });

            services.AddSingleton<EasyNetQ.IBus>((ctx) =>
            {
                return RabbitHutch.CreateBus(Configuration["RabbitMQConnection"]);
            });




            services.AddHttpClient<HealthCheckReportDownloaderService>("HealthCheckReportDownloader", options =>
            {
                // options.BaseAddress = new Uri(Configuration["CrmConnection:ServiceUrl"] + "api/data/v8.2/");
                options.Timeout = new TimeSpan(0, 2, 0);
                options.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                options.DefaultRequestHeaders.Add("OData-Version", "4.0");
                options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            }).ConfigurePrimaryHttpMessageHandler((ch) =>
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ClientCertificates.Add(GetCert());
                return handler;

            })
            //.AddHttpMessageHandler()
            // .AddHttpMessageHandler<OAuthTokenHandler>()
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            // services.AddHostedService<HealthCheckSubscribeService>();
        }


        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("MyPolicy");
            // app.UseAuthentication();
            app.UseAllAuthentication();

            app.UseHttpsRedirection();

            app.UseFileServer();
            app.UseStaticFiles();
            app.UseDefaultFiles();

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
            app.UseDefaultFiles();


            var HealthReportResult = new Schema
            {
                Properties = new Dictionary<string, Schema>
                                {
                                    {"name",new Schema{Type="string"}},
                                    {"status",new Schema{Type="string"}},
                                    {"duration",new Schema{Type="string"}},
                                    {"description",new Schema{Type="string"}},
                                    {"type",new Schema{Type="string"}},
                                    {"data",new Schema{Type="object"}},
                                    {"exception",new Schema{Type="string"}}
                                }
            };

            var HealthReport = new Schema
            {
                Properties = new Dictionary<string, Schema>
                        {
                            {"status",new Schema{Type="string"}},
                            {"duration",new Schema{Type="string"}},
                            {"results",new Schema{Type="array",
                                Items=HealthReportResult
                            }}
                        }
            };

            var Security = new List<IDictionary<string, IEnumerable<string>>>();
            Security.Add(new Dictionary<string, IEnumerable<string>>
            {
                { "BearerAuth", new List<string>()}
            });

            app.UseSwagger(e =>
            {
                e.PreSerializeFilters.Add((doc, req) =>
                {
                    doc.Definitions.Add("HealthReport", HealthReport);
                    doc.Paths.Add("/Health/IsAliveAndWell", new PathItem
                    {
                        Get = new Operation
                        {
                            Tags = new List<string> { "HealthCheck" },
                            Produces = new string[] { "application/json" },
                            Responses = new Dictionary<string, Response>{
                                {"200",new Response{Description="Success",
                                Schema = new Schema{Items=HealthReport}}},
                                {"503",new Response{Description="Failed"}}
                            },
                            Security = Security,
                            //     Parameters = new List<IParameter>{
                            //         new NonBodyParameter
                            //         {
                            //             Name = "Authorization",
                            //             In = "header",
                            //             Description = "access token",
                            //             Required = true,
                            //             Type = "string",
                            //             Default = "Bearer "
                            //         }
                            //    }
                        }
                    });

                    doc.Paths.Add("/Health/IsAlive", new PathItem
                    {
                        Get = new Operation
                        {
                            Tags = new List<string> { "HealthCheck" },
                            Produces = new string[] { "application/json" },
                            Responses = new Dictionary<string, Response>{
                                {"200",new Response{Description="Success",
                                Schema = new Schema{}}},
                                {"503",new Response{Description="Failed"}}
                            },
                            Security = Security,
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

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Home}/{action=Index}/{id?}");
            // });
            app.UseMvc();
            app.UseHealthChecksWithAuth("/Health/IsAliveAndWell", new HealthCheckOptions()
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

        public X509Certificate2 GetCert()
        {
            // //          "-----BEGIN CERTIFICATE-----"
            // var cert64 = "MIICYzCCAcygAwIBAgIBADANBgkqhkiG9w0BAQUFADAuMQswCQYDVQQGEwJVUzEMMAoGA1UEChMDSUJNMREwDwYDVQQLEwhMb2NhbCBDQTAeFw05OTEyMjIwNTAwMDBaFw0wMDEyMjMwNDU5NTlaMC4xCzAJBgNVBAYTAlVTMQwwCgYDVQQKEwNJQk0xETAPBgNVBAsTCExvY2FsIENBMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQD2bZEo7xGaX2/0GHkrNFZvlxBou9v1Jmt/PDiTMPve8r9FeJAQ0QdvFST/0JPQYD20rH0bimdDLgNdNynmyRoS2S/IInfpmf69iyc2G0TPyRvmHIiOZbdCd+YBHQi1adkj17NDcWj6S14tVurFX73zx0sNoMS79q3tuXKrDsxeuwIDAQABo4GQMIGNMEsGCVUdDwGG+EIBDQQ+EzxHZW5lcmF0ZWQgYnkgdGhlIFNlY3VyZVdheSBTZWN1cml0eSBTZXJ2ZXIgZm9yIE9TLzM5MCAoUkFDRikwDgYDVR0PAQH/BAQDAgAGMA8GA1UdEwEB/wQFMAMBAf8wHQYDVR0OBBYEFJ3+ocRyCTJw067dLSwr/nalx6YMMA0GCSqGSIb3DQEBBQUAA4GBAMaQzt+zaj1GU77yzlr8iiMBXgdQrwsZZWJo5exnAucJAEYQZmOfyLiMD6oYq+ZnfvM0n8G/Y79q8nhwvuxpYOnRSAXFp6xSkrIOeZtJMY1h00LKp/JX3Ng1svZ2agE126JHsQ0bhzN5TKsYfbwfTwfjdWAGy6Vf1nYi/rO+ryMO";
            // //"-----END CERTIFICATE----- "
            // var certbyte = Convert.FromBase64String(cert64);
            // X509Certificate2 cert = new X509Certificate2(certbyte);
            // var certsubject = cert.Subject;
            // return cert;

            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            //for some reason the cert collection .Find(...) method can't find a cert by thumprint so we created our own.
            var clientCert = FindCert(store, "53e0a006198c8939cd4f8469381d3cdcd2f93c2f");
            store.Dispose();
            return clientCert;
        }
        private static X509Certificate2 FindCert(X509Store store, string thumbprint)
        {
            foreach (var cert in store.Certificates)
                if (cert.Thumbprint.Equals(thumbprint,
                    StringComparison.CurrentCultureIgnoreCase))
                    return cert;
            return null;
        }
    }
}
