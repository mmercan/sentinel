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

namespace Sentinel.Handler.Product.Services
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
            this.bus = bus;
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
            try
            {
                 bus.Dispose();
            }
            catch (Exception ex)
            {
                logger.LogError("Exception: " + ex.Message);
            }
        }
    }
}
