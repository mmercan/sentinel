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