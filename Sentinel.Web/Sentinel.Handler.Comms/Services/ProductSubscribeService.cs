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

namespace Sentinel.Handler.Comms
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



            // var RabbitMQConn = configuration.GetSection("RabbitMQConnection").Value;
            // logger.LogCritical("Connecting to message queue url : " + RabbitMQConn);
            // _bus = RabbitHutch.CreateBus(RabbitMQConn);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            _executingTask = SubscribeProduct(_stoppingCts.Token);
            // If the task is completed then return it, this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }
            // Otherwise it's running
            return Task.CompletedTask;
        }
        private Task SubscribeProduct(CancellationToken cancellationToken)
        {
            var observer = Task.Factory.StartNew(() =>
   {

       try
       {
           var RabbitMQConn = configuration.GetSection("RabbitMQConnection").Value;
           logger.LogCritical("Async Connecting queue url : " + RabbitMQConn);
           using (var bus = RabbitHutch.CreateBus(RabbitMQConn))
           {
               logger.LogCritical("Connected to bus");
               // Change them
               // bus.Subscribe<ProductInfoDtoV2>("test", DoWork);
               bus.Subscribe<ProductInfoDtoV2>("productall", Handler, x => x.WithTopic("product.*"));
               Console.WriteLine("Listening on topic product.*");
               _ResetEvent.Wait();


               var cancelled = false;
               Console.CancelKeyPress += (_, e) =>
                   {
                       e.Cancel = true; // prevent the process from terminating.
                       cancelled = true;
                   };
           }
       }
       catch (Exception ex)
       {
           logger.LogError("Exception: " + ex.Message);
       }
   });
            return Task.CompletedTask;
        }
        private void Handler(ProductInfoDtoV2 state)
        {
            logger.LogCritical("ProductInfoDtoV2 Sync message " + state.Id);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogCritical("Timed Background Service is stopping.");
            _ResetEvent.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}