using System;
using System.Text;
using System.Threading;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

// using Microsoft.AspNetCore;
// using Microsoft.AspNetCore.Hosting;
using Sentinel.Handler.Comms.Services;

namespace Sentinel.Handler.Comms
{
    class Program_old
    {

        public static void Mainnn(string[] args)
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
                    services.AddSingleton<MonitorLoop>();

                    #region snippet1
                    // services.AddHostedService<TimedHostedService>();
                    #endregion

                    // #region snippet2
                    // services.AddHostedService<ConsumeScopedServiceHostedService>();
                    // services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
                    // #endregion

                    // #region snippet3
                    services.AddHostedService<ProductChangeService>();
                    //services.AddHostedService<ProductAsyncService>();
                    services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
                    // #endregion
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

                // Wait for the host to shutdown
                var waitforshutdown = host.WaitForShutdownAsync();
                waitforshutdown.Wait();
            }
        }
    }
}


// namespace Sentinel.Handler.Comms
// {
//     class Program
//     {

//         public static void Main(string[] args)
//         {
//             CreateWebHostBuilder(args).Build().Run();
//         }

//         public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
//             WebHost.CreateDefaultBuilder(args)
//                 .UseStartup<Startup>();
//     }
// }


// class Program
// {
//     public static IConfiguration Configuration;
//     public static ILoggerFactory LoggerFactory { get; private set; }
//     // public static ServiceCollection Services { get; private set; }
//     private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);

//     public static void Main(string[] args)
//     {
//         // Config();

//         var hostBuilder = new HostBuilder()
//             .ConfigureLogging((hostContext, config) =>
//             {
//                 config.AddConsole();
//                 config.AddDebug();
//             })
//             .ConfigureAppConfiguration((hostContext, config) =>
//             {
//                 config.AddEnvironmentVariables();
//                 config.AddJsonFile("appsettings.json", optional: true);
//                 config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
//                 config.AddCommandLine(args);
//             })

//         .ConfigureServices((hostContext, services) =>
//         {
//             // Config(services);
//             services.AddHostedService<TimedHostedService>();
//             // Add your services with depedency injection.

//             // services.AddSingleton<NatsSubscribe>();
//             // services.AddSingleton<ProductChangeSubscribe>();


//             // var provider = services.BuildServiceProvider();
//             // var natservice = provider.GetService<NatsSubscribe>();
//             // natservice.Subscribe();


//             // var productchange = provider.GetService<ProductChangeSubscribe>();
//             // productchange.Subscribe();
//         });

//         var task = hostBuilder.RunConsoleAsync();
//         task.Wait();

//     }

//     private static void Config(IServiceCollection services)
//     {
//         Console.OutputEncoding = Encoding.UTF8;
//         string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//         if (String.IsNullOrWhiteSpace(environment))
//             throw new ArgumentNullException("Environment not found in ASPNETCORE_ENVIRONMENT");
//         Console.WriteLine("Environment: {0}", environment);
//         // Services = new ServiceCollection();
//         // Set up configuration sources.
//         var builder = new ConfigurationBuilder()
//         .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
//             //.SetBasePath(Path.Combine(AppContext.BaseDirectory, "../../../"))
//             .AddJsonFile("appsettings.json", optional: false)
//             .AddEnvironmentVariables();

//         if (environment == "Development")
//         {
//             builder.AddJsonFile(
//                     Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.{environment}.json"),
//                     optional: true
//                 );
//         }

//         Configuration = builder.Build();
//         services.AddSingleton<IConfiguration>(Configuration);

//         // var loggerFactory = new LoggerFactory()
//         //     .AddConsole(Configuration.GetSection("Logging"))
//         //     .AddDebug();
//         // services.AddSingleton(this._hostingEnvironment);
//         // if (this._loggerFactory == null)
//         // 	this._loggerFactory = (ILoggerFactory) new LoggerFactory();
//         // services.AddSingleton<ILoggerFactory>(loggerFactory);
//         // services.AddLogging();
//     }
// }
// }


// private static void Listen()
// {
//     try
//     {
//         //Log.Info("Connecting to message queue url: {0}", Config.MessageQueueUrl);
//         using (var connection = new ConnectionFactory().CreateConnection("nats://localhost:4222/"))
//         {
//             var subscription = connection.SubscribeAsync("foo", QUEUE_GROUP);
//             subscription.MessageHandler += SaveViewer;
//             subscription.Start();
//             Console.WriteLine("Listening on subject: {0}, queue: {1}", "foo", QUEUE_GROUP);

//             _ResetEvent.WaitOne();
//             connection.Close();
//         }
//     }
//     catch (Exception ex)
//     {
//         System.Console.Error.WriteLine("Exception: " + ex.Message);
//         System.Console.Error.WriteLine(ex);
//     }
// }

// private static void SaveViewer(object sender, MsgHandlerEventArgs e)
// {
//     Console.WriteLine("Received message {0}", e.Message);
//     // var eventMessage = MessageHelper.FromData<ViewerSignedUpEvent>(e.Message.Data);
//     //  Console.WriteLine("Saving new viewer, signed up at: {0}; event ID: {1}", eventMessage.SignedUpAt, eventMessage.CorrelationId);

//     // var viewer = eventMessage.Viewer;
//     // using (var context = new WebinarContext())
//     // {
//     //     //reload child objects:
//     //     viewer.Country = context.Countries.Single(x => x.CountryCode == viewer.Country.CountryCode);
//     //     viewer.Role = context.Roles.Single(x => x.RoleCode == viewer.Role.RoleCode);
//     //     var interestCodes = viewer.Interests.Select(y => y.InterestCode);
//     //     viewer.Interests = context.Interests.Where(x => interestCodes.Contains(x.InterestCode)).ToList();

//     //     context.Viewers.Add(viewer);
//     //     context.SaveChanges();
//     // }
//     // Console.WriteLine("Viewer saved. Viewer ID: {0}; event ID: {1}", eventMessage.Viewer.ViewerId, eventMessage.CorrelationId);
// }


