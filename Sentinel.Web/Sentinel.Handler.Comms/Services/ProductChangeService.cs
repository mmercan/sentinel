using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Sentinel.Model.Product;
using Sentinel.Model.Product.Dto;
using Sentinel.Model;
using EasyNetQ;

namespace Sentinel.Handler.Comms.Services
{
    public class ProductChangeService : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        //  private Timer _timer;
        private IConfiguration configuration;

        public ProductChangeService(ILogger<TimedHostedService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            logger.LogCritical("ProductChangeService Contructor trigered");
            this.configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogCritical("RabbitmqSubscribe to message queue url");
                // using (var bus = RabbitHutch.CreateBus("host=sentinel-service-rabbitmq;username=rabbitmq;password=rabbitmq; timeout=10"))
                using (var bus = RabbitHutch.CreateBus("host=localhost;username=rabbitmq;password=rabbitmq; timeout=10"))
                {
                    logger.LogCritical("Connected to bus");
                    // bus.Subscribe<ProductInfo>("product", Handler, x => x.WithTopic("product.*"));



                    bus.Subscribe<ProductInfoDtoV2>("test", msg =>
                          logger.LogCritical("ProductInfoDtoV2 message " + msg.Id));

                    // bus.Respond<ProductInfoDtoV2, MessageResponse>(Responder);
                    // Console.WriteLine("Listening for (payment.*) messages. Hit <return> to quit.");
                    // Console.ReadLine();
                    // Console.WriteLine("terminated");
                }

            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine("Exception: " + ex.Message);
                System.Console.Error.WriteLine(ex);
            }


            return Task.CompletedTask;
        }
        public static MessageResponse Responder(ProductInfoDtoV2 request)
        {
            return new MessageResponse { Message = request.Id + "MEssage is here" };
        }


        private void Handler(ProductInfo product)
        {
            logger.LogCritical("Product Handler Service is working." + product.Id);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogCritical("Timed Background Service is stopping.");

            // _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            logger.LogCritical("ProductChangeService Disposed");
            // _timer?.Dispose();
        }
    }
}