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
    public class ProductAsyncSubscribeService : IHostedService, IDisposable
    {
        private ManualResetEventSlim _ResetEvent2 = new ManualResetEventSlim(false);
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
            // Store the task we're executing
            _executingTask = SubscribeAsync(_stoppingCts.Token);
            // If the task is completed then return it, this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }
            // Otherwise it's running
            return Task.CompletedTask;
        }

        private Task SubscribeAsync(CancellationToken cancellationToken)
        {

            var observer = Task.Factory.StartNew(() =>
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
                            // Dont catch this, it is caught further up the heirarchy and results in being sent to the default error queue
                            // on the broker
                            throw new EasyNetQException("Message processing exception - look in the default error queue (broker)");
                        }
                    }), x => x.WithTopic("product.newproduct"));
                   _ResetEvent2.Wait();
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
            logger.LogCritical("ProductInfoDtoV2 Async message " + state.Id);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogCritical("Timed Background Service is stopping.");
            _ResetEvent2.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}