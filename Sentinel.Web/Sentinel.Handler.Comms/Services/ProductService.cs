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
    public class ProductService : IHostedService, IDisposable
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private readonly ILogger logger;

        public ProductService(ILogger<ProductService> logger)
        {
            this.logger = logger;
            logger.LogCritical("TimedHostedService Contructor trigered");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogError("LogError Connecting to message queue url");
                using (var bus = RabbitHutch.CreateBus("host=sentinel-service-rabbitmq;username=rabbitmq;password=rabbitmq; timeout=10"))
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