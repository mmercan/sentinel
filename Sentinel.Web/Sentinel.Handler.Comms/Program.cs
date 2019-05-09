using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Sentinel.Model.Product.Dto;
using Sentinel.Model;
using EasyNetQ;
using Sentinel.Handler.Comms.Services;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Sentinel.Handler.Comms.ScheduledTask;
using Mercan.Common.ScheduledTask;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;

namespace Sentinel.Handler.Comms
{
    class Program
    {
        //public static IConfiguration Configuration { get; }
        public static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddCommandLine(args);
                    config.AddEnvironmentVariables();

                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddConsole();
                    logging.AddSerilog();
                    logging.AddDebug();
                })
                .ConfigureServices((hostContext, services) =>
                {

                    var env = hostContext.HostingEnvironment;

                    var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Enviroment", env.EnvironmentName)
                    .Enrich.WithProperty("ApplicationName", "Sentinel.Handler.Comms")
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .WriteTo.Console()
                    .WriteTo.File("Logs/logs.txt")
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
                    Log.Logger = logger.CreateLogger();
                    services.AddLogging();
                    services.AddSingleton<IConfiguration>(hostContext.Configuration);

                    // Scheduled Tasks Added                   
                    services.AddSingleton<IScheduledTask, SomeOtherTask>();
                    services.AddSingleton<IScheduledTask, QuoteOfTheDayTask>();
                    services.AddScheduler();

                    services.AddSingleton<IBus>(RabbitHutch.CreateBus(hostContext.Configuration.GetSection("RabbitMQConnection").Value));
                    services.AddHostedService<ProductSubscribeService>();
                    services.AddHostedService<ProductAsyncSubscribeService>();
                    services.AddHostedService<HealthMonitor>();
                    // services.AddHostedService<TimedHostedService>();
                    // services.AddSingleton<MonitorLoop>();
                    // services.AddHostedService<ConsumeScopedServiceHostedService>();
                    // services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
                    // services.AddHostedService<ProductChangeService>();
                    // services.AddHostedService<ProductAsyncService>();
                    // services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();




                    // services.Configure<HealthCheckPublisherOptions>(options =>
                    //    {
                    //        options.Delay = TimeSpan.FromSeconds(2);
                    //        // options.Predicate = (check) => check.Tags.Contains("ready");
                    //    });



                    CreateHealthCheck(services);
                    services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), typeof(HealthCheckPublisherOptions).Assembly.GetType("Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckPublisherHostedService")));

                })
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                var hoststart = host.StartAsync();
                hoststart.Wait();
                // Monitor for new background queue work items
                // var monitorLoop = host.Services.GetRequiredService<MonitorLoop>();
                // monitorLoop.StartMonitorLoop();
                var waitforshutdown = host.WaitForShutdownAsync();
                waitforshutdown.Wait();
            }
        }
        private static void CreateHealthCheck(IServiceCollection services)
        {
            var publishers = new TestPublisher[]
           {
                new TestPublisher(),
           };
            // private HealthCheckPublisherHostedService CreateService(
            //             IHealthCheckPublisher[] publishers, 
            //             Action<HealthCheckPublisherOptions> configure = null,
            //             TestSink sink = null)
            services.AddOptions();
            services.AddLogging();
                    services.AddHealthChecks()
                     .AddProcessList()
                     .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory")
                     .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "AvailableMBytes")
                     .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "PercentCommittedBytesInUse", "PercentCommittedBytesInUse_Base")
                     .AddSystemInfoCheck()
                     .AddWorkingSetCheck(10000000)
                     // .SqlConnectionHealthCheck(Configuration["SentinelConnection"])
                     // .AddApiIsAlive(Configuration.GetSection("sentinel-ui-sts:ClientOptions"), "api/healthcheck/isalive")
                     // .AddApiIsAlive(Configuration.GetSection("sentinel-api-member:ClientOptions"), "api/healthcheck/isalive")
                     // .AddApiIsAlive(Configuration.GetSection("sentinel-api-product:ClientOptions"), "api/healthcheck/isalive")
                     // .AddApiIsAlive(Configuration.GetSection("sentinel-api-comms:ClientOptions"), "api/healthcheck/isalive")
                     // .AddMongoHealthCheck(Configuration["Mongodb:ConnectionString"])
                     // .AddRabbitMQHealthCheck(Configuration["RabbitMQConnection"])
                     // .AddRedisHealthCheck(Configuration["RedisConnection"])
                     .AddDIHealthCheck(services);

            // Choosing big values for tests to make sure that we're not dependent on the defaults.
            // All of the tests that rely on the timer will set their own values for speed.
            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromMinutes(5);
                options.Period = TimeSpan.FromMinutes(5);
                options.Timeout = TimeSpan.FromMinutes(5);
            });

            if (publishers != null)
            {
                for (var i = 0; i < publishers.Length; i++)
                {
                    services.AddSingleton<IHealthCheckPublisher>(publishers[i]);
                }
            }

            // if (configure != null)
            // {
            //     services.Configure(configure);
            // }

            // if (sink != null)
            // {
            //     services.AddSingleton<ILoggerFactory>(new TestLoggerFactory(sink, enabled: true));
            // }



            // var serviceprov = services.BuildServiceProvider();
            // return serviceprov.GetServices<IHostedService>().OfType<HealthCheckPublisherHostedService>().Single();
        }

        static void HandleTextMessage(ProductInfoDtoV2 textMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Got message: {0}", textMessage.Id);
            Console.ResetColor();
        }
        
    }
    public class TestPublisher : IHealthCheckPublisher
        {
            private TaskCompletionSource<object> _started;

            public TestPublisher()
            {
                _started = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            }

            public List<(HealthReport report, CancellationToken cancellationToken)> Entries { get; } = new List<(HealthReport report, CancellationToken cancellationToken)>();

            public Exception Exception { get; set; }

            public Task Started => _started.Task;

            public Task Wait { get; set; }

            public async Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
            {
                Entries.Add((report, cancellationToken));

                // Signal that we've started
                _started.SetResult(null);

                if (Wait != null)
                {
                    await Wait;
                }

                if (Exception != null)
                {
                    throw Exception;
                }

                cancellationToken.ThrowIfCancellationRequested();
            }
        }
}