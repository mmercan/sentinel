using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Mercan.HealthChecks.Nats.Tests.Checks
{
    public class NatsShould
    {
        string connectionString = "nats://13.66.231.26:4222/";
        HealthCheckContext context = new HealthCheckContext();
        [Fact]
        public void CreateaNatsInstance()
        {
            var check = new NatsHealthCheck(connectionString);
        }

        [Fact]
        public async Task RunNatsHealthCheck()
        {
            var check = new NatsHealthCheck(connectionString);
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }
    }
}
