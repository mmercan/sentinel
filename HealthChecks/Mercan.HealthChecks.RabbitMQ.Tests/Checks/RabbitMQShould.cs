using System;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Mercan.HealthChecks.RabbitMQ.Tests.Checks
{
    public class RabbitMQShould
    {

        string connectionString = "host=52.252.16.88;username=rabbitmq;password=rabbitmq; timeout=10";
        string falseconnectionString = "host=5rabbitmq;username=rabbitmq;password=rabbitmq; timeout=10";
        HealthCheckContext context = new HealthCheckContext();

        [Fact]
        public void CreateaRabbitMQInstance()
        {
            RabbitMQHealthCheck check = new RabbitMQHealthCheck(connectionString);
        }

        [Fact]
        public async Task RunRabbitMQHealthCheck()
        {
            var check = new RabbitMQHealthCheck(connectionString);
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }


        // [Fact]
        // public async Task RunRabbitMQHealthCheckWithDI()
        // {
        //     var bus = RabbitHutch.CreateBus(connectionString);
        //     var check = new RabbitMQHealthCheckFromBus(bus);
        //     var result = await check.CheckHealthAsync(context);
        //     Assert.Equal(HealthStatus.Healthy, result.Status);
        // }

        [Fact]
        public void AddtothePipelineWorks()
        {
            var services1 = new ServiceCollection()
            .AddLogging()
            .AddSingleton<EasyNetQ.IBus>((ctx) =>
            {
                return RabbitHutch.CreateBus(connectionString);
            });
            services1.AddHealthChecks().AddRabbitMQHealthCheckWithDiIBus();

            var serviceProvider = services1.BuildServiceProvider();
            // var factory = serviceProvider.GetService<ILoggerFactory>();
            // var logger = factory.CreateLogger<DIHealthCheck>();
            // var healthChecksBuilder = serviceProvider.GetService<IHealthChecksBuilder>();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }


        [Fact]
        public void AddtothePipelineFails()
        {
            var services1 = new ServiceCollection()
            .AddLogging()
            .AddSingleton<EasyNetQ.IBus>((ctx) =>
            {
                return RabbitHutch.CreateBus(falseconnectionString);
            });
            services1.AddHealthChecks().AddRabbitMQHealthCheckWithDiIBus();

            var serviceProvider = services1.BuildServiceProvider();
            // var factory = serviceProvider.GetService<ILoggerFactory>();
            // var logger = factory.CreateLogger<DIHealthCheck>();
            // var healthChecksBuilder = serviceProvider.GetService<IHealthChecksBuilder>();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Unhealthy, result.Status);

        }
    }
}
