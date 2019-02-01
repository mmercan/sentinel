using System;
using System.Text;
using System.Threading;
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
                    // services.AddHostedService<TimedHostedService>();
                    // services.AddSingleton<MonitorLoop>();
                    // services.AddHostedService<ConsumeScopedServiceHostedService>();
                    // services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
                    // services.AddHostedService<ProductChangeService>();
                    // services.AddHostedService<ProductAsyncService>();
                    // services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
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

        static void HandleTextMessage(ProductInfoDtoV2 textMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Got message: {0}", textMessage.Id);
            Console.ResetColor();
        }
    }
}