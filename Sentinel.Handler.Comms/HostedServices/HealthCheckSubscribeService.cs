using System;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using Mercan.HealthChecks.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace SSentinel.Handler.Comms.HostedServices
{
    public class HealthCheckSubscribeService : IHostedService, IDisposable
    {

        private string subscriptionId = "HealthCheck";
        IBus bus;
        private ManualResetEventSlim _ResetEvent = new ManualResetEventSlim(false);
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        public HealthCheckSubscribeService(ILogger<HealthCheckSubscribeService> logger, IConfiguration configuration, IBus bus)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.bus = bus;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = Task.Factory.StartNew(new Action(SubscribeToTopicAsync), TaskCreationOptions.LongRunning);
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }
            return Task.CompletedTask;
        }

        private void SubscribeToTopicAsync()
        {
            try
            {
                logger.LogCritical(this.GetType().FullName + " Async Connected to bus");
                bus.SubscribeAsync<ListResponse>(subscriptionId, message => Task.Factory.StartNew(() =>
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
                 }), x => x.WithTopic(subscriptionId));
                _ResetEvent.Wait();
            }
            catch (Exception ex)
            {
                logger.LogError("Exception: " + ex.Message);
            }
        }
        private void Handler(ListResponse state)
        {
            logger.LogCritical(this.GetType().FullName + " Async message ");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogCritical(this.GetType().FullName + " Service is stopping.");
            _ResetEvent.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

    }
}