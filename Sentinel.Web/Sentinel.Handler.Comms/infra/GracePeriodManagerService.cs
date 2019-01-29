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

namespace Sentinel.Handler.Comms
{
    public class GracePeriodManagerService
           : BackgroundService
    {
        private readonly ILogger<GracePeriodManagerService> _logger;
        //private readonly OrderingBackgroundSettings _settings;

        private readonly IEventBus _eventBus;

        public GracePeriodManagerService(
                                         IEventBus eventBus,
                                         ILogger<GracePeriodManagerService> logger)
        {
            //Constructor’s parameters validations... 
            _logger = logger;
            _eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"GracePeriodManagerService is starting.");

            stoppingToken.Register(() =>
                    _logger.LogDebug($" GracePeriod background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"GracePeriod task doing background work.");

                // This eShopOnContainers method is quering a database table 
                // and publishing events into the Event Bus (RabbitMS / ServiceBus)
                // CheckConfirmedGracePeriodOrders();

                await Task.Delay(new TimeSpan(0, 0, 20), stoppingToken);
            }

            _logger.LogDebug($"GracePeriod background task is stopping.");
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            // Run your graceful clean-up actions
        }
    }
}