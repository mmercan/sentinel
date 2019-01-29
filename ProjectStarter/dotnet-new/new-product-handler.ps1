$folder = "Sentinel.Handler.Product"
Write-Host "--------------------------------"
$scriptpath = $MyInvocation.MyCommand.Path 
$dir = Split-Path $scriptpath
$appRootFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\
$appFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\$folder

set-location -Path $appFolder
dotnet new console


dotnet add package "Microsoft.Extensions.Configuration.CommandLine"
dotnet add package "Microsoft.Extensions.Configuration.EnvironmentVariables"
dotnet add package "Microsoft.Extensions.Configuration.Json"
dotnet add package "Microsoft.Extensions.Hosting"
dotnet add package "Microsoft.Extensions.Hosting.Abstractions"
dotnet add package "Microsoft.Extensions.Logging"
dotnet add package "Microsoft.Extensions.Logging.Configuration"
dotnet add package "Microsoft.Extensions.Logging.Console"
dotnet add package "Microsoft.Extensions.Logging.Debug"
dotnet add package "NATS.Client"
dotnet add package "STAN.Client"
dotnet add package "EasyNetQ"

dotnet add package "Polly"

dotnet add package "Serilog"
dotnet add package "Serilog.Extensions.Logging"
# dotnet add package "Serilog.AspNetCore"
dotnet add package "Serilog.Sinks.Console"
dotnet add package "Serilog.Sinks.File"
#  dotnet add package "Serilog.Sinks.LogstashHttp"
dotnet add package "Serilog.Settings.Configuration"
dotnet add package "Serilog.Sinks.Loggly"
dotnet add package "Serilog.Sinks.ElasticSearch"




function update-Programcs {

    $fileName = "$appFolder\Program.cs"
    (Get-Content $fileName) |
        Foreach-Object {
        if ($_ -match "Hello World!") {

            'var host = new HostBuilder()'
            '    .ConfigureAppConfiguration((hostContext, config) =>'
            '    {'
            '        config.AddJsonFile("appsettings.json", optional: true);'
            '        config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);'
            '        config.AddCommandLine(args);'
            '        config.AddEnvironmentVariables();'
            '    })'
            '    .ConfigureLogging((hostContext, logging) =>'
            '    {'
            '        logging.AddConsole();'
            '        logging.AddSerilog();'
            '        logging.AddDebug();'
            '    })'
            '    .ConfigureServices((hostContext, services) =>'
            '    {'
            '        var env = hostContext.HostingEnvironment;'
            '        var logger = new LoggerConfiguration()'
            '        .ReadFrom.Configuration(hostContext.Configuration)'
            '        .Enrich.FromLogContext()'
            '        .Enrich.WithProperty("Enviroment", env.EnvironmentName)'
            '        .Enrich.WithProperty("ApplicationName", "Api App")'
            '        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)'
            '        .WriteTo.Console()'
            '        .WriteTo.File("Logs/logs.txt")'
            '        //.WriteTo.Kafka(new KafkaSinkOptions(topic: "test", brokers: new[] { new Uri(Configuration["KafkaUrl"]) }))'
            '        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://sentinel-db-elasticsearch:9200"))'
            '        {'
            '            AutoRegisterTemplate = true,'
            '            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,'
            '            TemplateName = "productslog",'
            '            IndexFormat = "productslog-{0:yyyy.MM.dd}",'
            '            InlineFields = true,'
            '            // IndexDecider = (@event, offset) => "test_elapsedtimes",'
            '            CustomFormatter = new ElasticsearchJsonFormatter()'
            '        });'
            '        logger.WriteTo.Console();'
            '        Log.Logger = logger.CreateLogger();'
            '        services.AddLogging();'
            '        services.AddSingleton<IConfiguration>(hostContext.Configuration);'
            '        services.AddHostedService<ProductSubscribeService>();'
            '        // services.AddSingleton<MonitorLoop>();'
            '        // services.AddHostedService<ConsumeScopedServiceHostedService>();'
            '        // services.AddScoped<IScopedProcessingService, ScopedProcessingService>();'
            '        // services.AddHostedService<ProductChangeService>();'
            '        // services.AddHostedService<ProductAsyncService>();'
            '        // services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();'
            '    })'
            '    .UseConsoleLifetime()'
            '    .Build();'
            'using (host)'
            '{'
            '    // Start the host'
            '    var hoststart = host.StartAsync();'
            '    hoststart.Wait();'
            '    // Monitor for new background queue work items'
            '    //  var monitorLoop = host.Services.GetRequiredService<MonitorLoop>();'
            '    //  monitorLoop.StartMonitorLoop();'
            '    var waitforshutdown = host.WaitForShutdownAsync();'
            '    waitforshutdown.Wait();'
            '}'
        }
    } | Set-Content $fileName

}

update-Programcs

new-item -type directory -path $appFolder\Services -Force

function update-ProductSubscribeService {

$ProductSubscribeService = @'
using System;
using System.Threading;
using NATS.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sentinel.Model.Product;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Sentinel.Model.Product.Dto;
using Sentinel.Model;
using Microsoft.Extensions.Configuration;

namespace __projectname__
{
    public class ProductSubscribeService : IHostedService, IDisposable
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        public ProductSubscribeService(ILogger<ProductSubscribeService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            logger.LogCritical("TimedHostedService Contructor trigered");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var RabbitMQConn = configuration.GetSection("RabbitMQConnection").Value;
                logger.LogCritical("Connecting to message queue url : " + RabbitMQConn);
                using (var bus = RabbitHutch.CreateBus(RabbitMQConn))
                {
                    logger.LogCritical("Connected to bus");
                    bus.Subscribe<ProductInfoDtoV2>("test", msg =>
                          logger.LogCritical("ProductInfoDtoV2 message " + msg.Id));

                    Console.WriteLine("Listening on subject", "foo");
                    _ResetEvent.WaitOne();
                }
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine("Exception: " + ex.Message);
                System.Console.Error.WriteLine(ex);
            }
            return Task.CompletedTask;

        }

        private void DoWork(object state)
        {
            // _logger.LogCritical("Timed Background Service is working.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogCritical("Timed Background Service is stopping.");
            _ResetEvent.Close();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}


'@
$ProductSubscribeService = $ProductSubscribeService.replace("__projectname__", $folder)
$ProductSubscribeService | set-content  ".\Services\ProductSubscribeService.cs"

}




function update-ProductAsyncSubscribeService {

$ProductAsyncSubscribeService = @'
using System;
using System.Threading;
using NATS.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sentinel.Model.Product;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Sentinel.Model.Product.Dto;
using Sentinel.Model;
using Microsoft.Extensions.Configuration;

namespace __projectname__
{
    public class ProductSubscribeService : IHostedService, IDisposable
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        public ProductSubscribeService(ILogger<ProductSubscribeService> logger, IConfiguration configuration)
        {
                this.logger = logger;
                this.configuration = configuration;
                logger.LogCritical("TimedHostedService Contructor trigered");
            }
    
            public Task StartAsync(CancellationToken cancellationToken)
            {
                try
                {
                    var RabbitMQConn = configuration.GetSection("RabbitMQConnection").Value;
                    logger.LogCritical("Connecting to message queue url : " + RabbitMQConn);
                    using (var bus = RabbitHutch.CreateBus(RabbitMQConn))
                    {
                        logger.LogCritical("Connected to bus");
                        bus.Subscribe<ProductInfoDtoV2>("test", msg =>
                              logger.LogCritical("ProductInfoDtoV2 message " + msg.Id));
    
                        Console.WriteLine("Listening on subject", "foo");
                        _ResetEvent.WaitOne();
                    }
                }
                catch (Exception ex)
                {
                    System.Console.Error.WriteLine("Exception: " + ex.Message);
                    System.Console.Error.WriteLine(ex);
                }
                return Task.CompletedTask;
    
            }
    
            private void DoWork(object state)
            {
                // _logger.LogCritical("Timed Background Service is working.");
            }
    
            public Task StopAsync(CancellationToken cancellationToken)
            {
                logger.LogCritical("Timed Background Service is stopping.");
                _ResetEvent.Close();
                return Task.CompletedTask;
            }
    
            public void Dispose()
            {
            }
        }
    }
    
'@

$ProductAsyncSubscribeService = $ProductAsyncSubscribeService.replace("__projectname__", $folder)
$ProductAsyncSubscribeService | set-content  ".\Services\ProductAsyncSubscribeService.cs"

}
    
    