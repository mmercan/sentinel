using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Mercan.HealthChecks.RabbitMQ.Tests.Checks
{
    public class RabbitMQShould
    {

        string connectionString = "host=52.183.35.232;username=rabbitmq;password=rabbitmq; timeout=10";
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
    }
}
