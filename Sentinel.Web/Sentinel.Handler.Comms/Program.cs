using System;
using System.Text;
using System.Threading;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sentinel.Handler.Comms
{
    class Program
    {
        public static IConfiguration Configuration;
        public static ILoggerFactory LoggerFactory { get; private set; }
        public static ServiceCollection Services { get; private set; }



        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);

        public static void Main(string[] args)
        {
            Config();

            Services.AddSingleton<NatsSubscribe>();
            var provider = Services.BuildServiceProvider();
            var natservice = provider.GetService<NatsSubscribe>();
            natservice.Subscribe();
            // Listen();
        }

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
        private static void Config()
        {
            Console.OutputEncoding = Encoding.UTF8;

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (String.IsNullOrWhiteSpace(environment))
                throw new ArgumentNullException("Environment not found in ASPNETCORE_ENVIRONMENT");

            Console.WriteLine("Environment: {0}", environment);

            Services = new ServiceCollection();

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))

                //.SetBasePath(Path.Combine(AppContext.BaseDirectory, "../../../"))
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables();

            if (environment == "Development")
            {
                builder
                    .AddJsonFile(
                        Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.{environment}.json"),
                        optional: true
                    );
            }

            Configuration = builder.Build();
            Services.AddSingleton<IConfiguration>(Configuration);

            var loggerFactory = new LoggerFactory()
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug();


            // services.AddSingleton(this._hostingEnvironment);
            // if (this._loggerFactory == null)
            // 	this._loggerFactory = (ILoggerFactory) new LoggerFactory();

            // using (List<Action>.Enumerator enumerator = this._configureLoggingDelegates.GetEnumerator())
            // {
            // 	while (enumerator.MoveNext())
            // 		enumerator.Current(this._loggerFactory);
            // }


            Services.AddSingleton<IConfiguration>(Configuration);
            Services.AddSingleton<ILoggerFactory>(loggerFactory);
            Services.AddLogging();



        }


    }
}

