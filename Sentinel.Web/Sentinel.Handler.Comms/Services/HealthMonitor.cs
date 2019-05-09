using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Sentinel.Model.Product;
using Sentinel.Model.Product.Dto;
using Sentinel.Model;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Sentinel.Handler.Comms.Services
{
    public class HealthMonitor : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        //  private Timer _timer;
        private IConfiguration configuration;
        HealthCheckService healthCheckService;

        public HealthMonitor(ILogger<TimedHostedService> logger, HealthCheckService healthCheckService, IConfiguration configuration)
        {
            this.logger = logger;
            logger.LogCritical("HealthMonitor Contructor trigered");
            this.configuration = configuration;
            this.healthCheckService = healthCheckService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
         {
             try
             {
                 var reporttask = healthCheckService.CheckHealthAsync(null, cancellationToken);
                 reporttask.Wait();
                 var result = reporttask.Result;
                 string json = JsonConvert.SerializeObject(result, Formatting.Indented);
                 logger.LogCritical("HealthMonitor Completed");
                 logger.LogCritical(json);
             }
             catch (Exception ex)
             {
                 System.Console.Error.WriteLine("Exception: " + ex.Message);
                 System.Console.Error.WriteLine(ex);
             }
             //return Task.CompletedTask;
         });
        }
        public static MessageResponse Responder(ProductInfoDtoV2 request)
        {
            return new MessageResponse { Message = request.Id + "Message is here" };
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogCritical("Timed Background Service is stopping.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            logger.LogCritical("HealthMonitor Disposed");
            // _timer?.Dispose();
        }
    }
}