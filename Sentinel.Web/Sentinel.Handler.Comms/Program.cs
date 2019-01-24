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

namespace Sentinel.Handler.Comms
{
    class Program
    {
        public static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureLogging((hostContext, config) =>
                {
                    config.AddConsole();
                    config.AddDebug();
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddEnvironmentVariables();
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    // services.AddSingleton<MonitorLoop>();
                    services.AddHostedService<ProductService>();
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
                // Start the host
                var hoststart = host.StartAsync();
                hoststart.Wait();
                // Monitor for new background queue work items
                //  var monitorLoop = host.Services.GetRequiredService<MonitorLoop>();
                //  monitorLoop.StartMonitorLoop();
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