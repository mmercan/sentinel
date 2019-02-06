# Import-Module .\new-dotnet.ps1 -Force

# $folder = "Sentinel.Handler.Product"
# Write-Host "--------------------------------"
# $scriptpath = $MyInvocation.MyCommand.Path 
# $dir = Split-Path $scriptpath
# $appRootFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\
# $appFolder = Join-Path -Path $dir -ChildPath ..\..\Sentinel.Web\$folder

# set-location -Path $appFolder
function new-consoleApp {


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
    dotnet add package "Serilog"
    dotnet add package "Serilog.Extensions.Logging"
    dotnet add package "Serilog.Settings.Configuration"
    dotnet add package "Serilog.Sinks.Console"
    dotnet add package "Serilog.Sinks.ElasticSearch"
    dotnet add package "Serilog.Sinks.File"
    dotnet add package "Serilog.Sinks.Loggly"
    dotnet add package "EasyNetQ"
    dotnet add package "Polly"
    dotnet add package "ncrontab"

    dotnet add reference "..\Sentinel.Common\Sentinel.Model\Sentinel.Model.csproj"
    dotnet add reference "..\Mercan\Mercan.Common\Mercan.Common.csproj"

    dotnet restore
    dotnet build

}

function update-handler-Programcs {

    $fileName = "$appFolder\Program.cs"
    $programobj = (Get-Content $fileName) |
        Foreach-Object {
        $_ # send the current line to output
        if ($_ -match "using System;") {

            'using System.Text;'
            'using System.Threading;'
            'using System.IO;'
            'using Microsoft.Extensions.Configuration;'
            'using Microsoft.Extensions.DependencyInjection;'
            'using Microsoft.Extensions.Logging;'
            'using Microsoft.Extensions.Hosting;'
            'using Sentinel.Model.Product.Dto;'
            'using Sentinel.Model;'
            'using EasyNetQ;'
            'using __projectname__.Services;'
            'using Serilog;'
            'using Serilog.Events;'
            'using Serilog.Sinks.Elasticsearch;'
            'using __projectname__.ScheduledTask;'
            'using Mercan.Common.ScheduledTask;'
                
        }

        if ($_ -match "Hello World!") {

            'var host = new HostBuilder()'
            '.ConfigureAppConfiguration((hostContext, config) =>'
            '{'
            '    config.AddJsonFile("appsettings.json", optional: true);'
            '    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);'
            '    config.AddCommandLine(args);'
            '    config.AddEnvironmentVariables();'
            '})'
            '.ConfigureLogging((hostContext, logging) =>'
            '{'
            '    logging.AddConsole();'
            '    logging.AddSerilog();'
            '    logging.AddDebug();'
            '})'
            '.ConfigureServices((hostContext, services) =>'
            '{'
            '    var env = hostContext.HostingEnvironment;'
            '    var logger = new LoggerConfiguration()'
            '    .ReadFrom.Configuration(hostContext.Configuration)'
            '    .Enrich.FromLogContext()'
            '    .Enrich.WithProperty("Enviroment", env.EnvironmentName)'
            '    .Enrich.WithProperty("ApplicationName", "Sentinel.Handler.Comms")'
            '    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)'
            '    .WriteTo.Console()'
            '    .WriteTo.File("Logs/logs.txt")'
            '    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://sentinel-db-elasticsearch:9200"))'
            '    {'
            '        AutoRegisterTemplate = true,'
            '        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,'
            '        TemplateName = "productslog",'
            '        IndexFormat = "productslog-{0:yyyy.MM.dd}",'
            '        InlineFields = true,'
            '        // IndexDecider = (@event, offset) => "test_elapsedtimes",'
            '        CustomFormatter = new ElasticsearchJsonFormatter()'
            '    });'
            '    logger.WriteTo.Console();'
            '    Log.Logger = logger.CreateLogger();'
            '    services.AddLogging();'
            '    services.AddSingleton<IConfiguration>(hostContext.Configuration);'
            '    // Scheduled Tasks Added             '      
            '    services.AddSingleton<IScheduledTask, SomeOtherTask>();'
            '    services.AddSingleton<IScheduledTask, QuoteOfTheDayTask>();'
            '    services.AddScheduler();'
            '    services.AddSingleton<IBus>(RabbitHutch.CreateBus(hostContext.Configuration.GetSection("RabbitMQConnection").Value));'
            '    services.AddHostedService<ProductSubscribeService>();'
            '    services.AddHostedService<ProductAsyncSubscribeService>();'
            '    // services.AddHostedService<TimedHostedService>();'
            '    // services.AddSingleton<MonitorLoop>();'
            '    // services.AddHostedService<ConsumeScopedServiceHostedService>();'
            '    // services.AddScoped<IScopedProcessingService, ScopedProcessingService>();'
            '    // services.AddHostedService<ProductChangeService>();'
            '    // services.AddHostedService<ProductAsyncService>();'
            '    // services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();'
            '})'
            '.UseConsoleLifetime()'
            '.Build();'
            'using (host)'
            '{'
            '    var hoststart = host.StartAsync();'
            '    hoststart.Wait();'
            '    // Monitor for new background queue work items'
            '    // var monitorLoop = host.Services.GetRequiredService<MonitorLoop>();'
            '    // monitorLoop.StartMonitorLoop();'
            '    var waitforshutdown = host.WaitForShutdownAsync();'
            '    waitforshutdown.Wait();'
            '}'

        }


    } 
    $programobj = $programobj.replace("__projectname__", $folder)
    $programobj | set-content  $fileName

}

function update-handler-ProductSubscribeService {

    $ProductSubscribeService = @'
    using System;
    using System.Threading;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Sentinel.Model.Product;
    using EasyNetQ;
    using Microsoft.Extensions.Hosting;
    using System.Threading.Tasks;
    using Sentinel.Model.Product.Dto;
    using Sentinel.Model;
    
    namespace __projectname__.Services
    {
        public class ProductSubscribeService : IHostedService, IDisposable
        {
            IBus bus;
            private ManualResetEventSlim _ResetEvent = new ManualResetEventSlim(false);
            private readonly ILogger logger;
            private readonly IConfiguration configuration;
            private Task _executingTask;
            private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
            public ProductSubscribeService(ILogger<ProductSubscribeService> logger, IConfiguration configuration, IBus bus)
            {
                this.logger = logger;
                this.configuration = configuration;
                this.bus = bus;
            }
    
            public Task StartAsync(CancellationToken cancellationToken)
            {
                _executingTask = Task.Factory.StartNew(new Action(SubscribeProduct), TaskCreationOptions.LongRunning);
                if (_executingTask.IsCompleted)
                {
                    return _executingTask;
                }
                return Task.CompletedTask;
            }
            private void SubscribeProduct()
            {
                try
                {
                    var RabbitMQConn = configuration.GetSection("RabbitMQConnection").Value;
                    logger.LogCritical("Async Connecting queue url : " + RabbitMQConn);
                    using (var bus = RabbitHutch.CreateBus(RabbitMQConn))
                    {
                        logger.LogCritical("Connected to bus");
                        bus.Subscribe<ProductInfoDtoV2>("productall", Handler, x => x.WithTopic("product.*"));
                        Console.WriteLine("Listening on topic product.*");
                        _ResetEvent.Wait();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Exception: " + ex.Message);
                }
            }
            private void Handler(ProductInfoDtoV2 state)
            {
                logger.LogCritical("ProductInfoDtoV2 Sync message " + state.Id);
            }
    
            public Task StopAsync(CancellationToken cancellationToken)
            {
                logger.LogCritical("ProductSubscribeService Service is stopping.");
                _ResetEvent.Dispose();
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

function update-handler-ProductAsyncSubscribeService {

    $ProductAsyncSubscribeService = @'
    using System;
    using System.Threading;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Sentinel.Model.Product;
    using EasyNetQ;
    using Microsoft.Extensions.Hosting;
    using System.Threading.Tasks;
    using Sentinel.Model.Product.Dto;
    using Sentinel.Model;
    
    namespace __projectname__.Services
    {
        public class ProductAsyncSubscribeService : IHostedService, IDisposable
        {
            private ManualResetEventSlim _ResetEvent = new ManualResetEventSlim(false);
            private IBus bus;
            private readonly ILogger logger;
            private readonly IConfiguration configuration;
            private Task _executingTask;
            private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
            public ProductAsyncSubscribeService(ILogger<ProductAsyncSubscribeService> logger, IConfiguration configuration, IBus bus)
            {
                this.logger = logger;
                this.configuration = configuration;
                //this.bus = bus;
            }
    
            public Task StartAsync(CancellationToken cancellationToken)
            {
                _executingTask = Task.Factory.StartNew(new Action(SubscribeProductAsync), TaskCreationOptions.LongRunning);
                if (_executingTask.IsCompleted)
                {
                    return _executingTask;
                }
                return Task.CompletedTask;
            }
    
            private void SubscribeProductAsync()
            {
                try
                {
                    var RabbitMQConn = configuration.GetSection("RabbitMQConnection").Value;
                    logger.LogCritical("Async Connecting queue url : " + RabbitMQConn);
                    using (var bus = RabbitHutch.CreateBus(RabbitMQConn))
                    {
                        logger.LogCritical("Async Connected to bus");
                        bus.SubscribeAsync<ProductInfoDtoV2>("product", message => Task.Factory.StartNew(() =>
                         {
                             Handler(message);
                         }).ContinueWith(task =>
                         {
                             if (task.IsCompleted && !task.IsFaulted)
                             {
                                 logger.LogCritical("Finished processing all messages");
                             }
                             else
                             {
                                 throw new EasyNetQException("Message processing exception - look in the default error queue (broker)");
                             }
                         }), x => x.WithTopic("product.newproduct"));
                        _ResetEvent.Wait();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Exception: " + ex.Message);
                }
            }
            private void Handler(ProductInfoDtoV2 state)
            {
                logger.LogCritical("ProductInfoDtoV2 Async message " + state.Id);
            }
    
            public Task StopAsync(CancellationToken cancellationToken)
            {
                logger.LogCritical("ProductAsyncSubscribeService Service is stopping.");
                _ResetEvent.Dispose();
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

function update-handler-QuoteOfTheDayTask {

    $QuoteOfTheDayTask = @'
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Microsoft.Extensions.Logging;
    using Mercan.Common.ScheduledTask;
    
    namespace __projectname__.ScheduledTask
    {
        // uses https://theysaidso.com/api/
        public class QuoteOfTheDayTask : IScheduledTask
        {
            private ILogger<QuoteOfTheDayTask> _logger;
            public QuoteOfTheDayTask(ILogger<QuoteOfTheDayTask> logger)
            {
                _logger = logger;
            }
            public string Schedule => "*/10 * * * *"; // "*/10 * * * *" //Runs every 10 minutes;
            public async Task ExecuteAsync(CancellationToken cancellationToken)
            {
                _logger.LogCritical("QuoteOfTheDayTask triggered");
                var httpClient = new HttpClient();
                var quoteJson = JObject.Parse(await httpClient.GetStringAsync("http://quotes.rest/qod.json"));
                QuoteOfTheDay.Current = JsonConvert.DeserializeObject<QuoteOfTheDay>(quoteJson["contents"]["quotes"][0].ToString());
                _logger.LogCritical(QuoteOfTheDay.Current.Quote);
            }
        }
    
        public class QuoteOfTheDay
        {
            public static QuoteOfTheDay Current { get; set; }
            static QuoteOfTheDay()
            {
                Current = new QuoteOfTheDay { Quote = "No quote", Author = "Maarten" };
            }
            public string Quote { get; set; }
            public string Author { get; set; }
        }
    }

'@
    $QuoteOfTheDayTask = $QuoteOfTheDayTask.replace("__projectname__", $folder)
    $QuoteOfTheDayTask | set-content  ".\ScheduledTasks\QuoteOfTheDayTask.cs"

}

function update-handler-SomeOtherTask {

    $SomeOtherTask = @'
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Mercan.Common.ScheduledTask;
    namespace __projectname__.ScheduledTask
    {
        public class SomeOtherTask : IScheduledTask
        {
            private ILogger<SomeOtherTask> _logger;
            public SomeOtherTask(ILogger<SomeOtherTask> logger)
            {
                _logger = logger;
            }
            public string Schedule => "0 5 * * *"; // "*/10 * * * *" //Runs every 10 minutes;
            public async Task ExecuteAsync(CancellationToken cancellationToken)
            {
                _logger.LogCritical("SomeOtherTask triggered");
                await Task.Delay(5000, cancellationToken);
            }
        }
    }

'@
    $SomeOtherTask = $SomeOtherTask.replace("__projectname__", $folder)
    $SomeOtherTask | set-content  ".\ScheduledTasks\SomeOtherTask.cs"

}


function update-handler-appsettings {

    $appsettings = @'
    {
        "Logging": {
            "LogLevel": {
                "Default": "Warning"
            }
        },
        "NATS_URL": "nats://localhost:4222/",
        "NATS_COMM_SUBJECT": "comms",
        "NATS_COMM_QUEUE_GROUP": "comms-handler",
        "RabbitMQConnection": "host=localhost;username=rabbitmq;password=rabbitmq; timeout=10"
    
    }

'@
    # $appsettings = $appsettings.replace("__projectname__", $folder)
    $appsettings | set-content  ".\appsettings.json"




    $xmlfile = "$appFolder/Sentinel.Handler.Product.csproj"
    [XML]$xml = Get-Content $xmlfile
    $newNode = $xml.CreateElement('ItemGroup')

    $itemGroupnode = $xml.CreateElement('None')
    $itemGroupnode.SetAttribute('Update', 'appsettings.json')

    $copyToOutput = $xml.CreateElement('CopyToOutputDirectory')
    $copyToOutput.InnerText = "Always"

    $itemGroupnode.AppendChild($copyToOutput)
    $newNode.AppendChild($itemGroupnode)
    $xml.Project.AppendChild($newnode)

    $xml.Save($xmlfile)

}



# update-handler-Programcs

# new-item -type directory -path $appFolder\Services -Force
# new-item -type directory -path $appFolder\ScheduledTasks -Force

# update-handler-ProductAsyncSubscribeService
# update-handler-ProductSubscribeService
# update-handler-SomeOtherTask
# update-handler-QuoteOfTheDayTask
# update-handler-appsettings

# dotnet build ../Sentinel.Web.sln
# dotnet build ../Sentinel.Web.sln
# dotnet restore
# dotnet build



# Add-Dockerfile

# cd $scriptpath
