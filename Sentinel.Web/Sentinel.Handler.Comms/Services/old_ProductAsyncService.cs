// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Configuration;
// using Sentinel.Model.Product;
// using Sentinel.Model.Product.Dto;
// using Sentinel.Model;
// using EasyNetQ;
// using System.Collections.Concurrent;

// namespace Sentinel.Handler.Comms.Services
// {
//     public class OldMyWorker
//     {
//         public MessageResponse Execute(ProductInfoDtoV2 request)
//         {
//             MessageResponse responseMessage = new MessageResponse();
//             responseMessage.Message = "1234";
//             Console.WriteLine("Worker activated to process response.");

//             return responseMessage;
//         }
//     }

//     public class OldProductAsyncService : IHostedService, IDisposable
//     {
//         private readonly ILogger logger;
//         //  private Timer _timer;
//         private IConfiguration configuration;

//         public ProductAsyncService(ILogger<TimedHostedService> logger, IConfiguration configuration)
//         {
//             this.logger = logger;
//             logger.LogCritical("ProductChangeAsync Contructor trigered");
//             this.configuration = configuration;
//         }

//         public Task StartAsync(CancellationToken cancellationToken)
//         {
//             var workers = new BlockingCollection<MyWorker>();
//             for (int i = 0; i < 10; i++)
//             {
//                 workers.Add(new MyWorker());
//             }

//             using (var bus = RabbitHutch.CreateBus("host=sentinel-service-rabbitmq;username=rabbitmq;password=rabbitmq"))
//             {
//                 logger.LogCritical("ProductChangeAsync Bus running");
//                 bus.RespondAsync<ProductInfoDtoV2, MessageResponse>(request =>
//                     Task.Factory.StartNew(() =>
//                     {
//                         var worker = workers.Take();
//                         try
//                         {
//                             return worker.Execute(request);
//                         }
//                         finally
//                         {
//                             workers.Add(worker);
//                         }
//                     }));

//                 Console.WriteLine("Listening for messages. Hit <return> to quit.");
//                 Console.ReadLine();
//             }

//             return Task.CompletedTask;
//         }



//         public Task StopAsync(CancellationToken cancellationToken)
//         {
//             logger.LogCritical("Timed Background Service is stopping.");

//             // _timer?.Change(Timeout.Infinite, 0);

//             return Task.CompletedTask;
//         }

//         public void Dispose()
//         {
//             logger.LogCritical("ProductChangeService Disposed");
//             // _timer?.Dispose();
//         }
//     }
// }