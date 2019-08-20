using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Mercan.HealthChecks.Network.Tests.Checks
{
    public class NetworkShould
    {
        string connectionString = "nats://13.77.147.26:4222/";
        HealthCheckContext context = new HealthCheckContext();

        [Fact]
        public void CreateaNetworkInstance()
        {
            NetworkHealthCheck check = new NetworkHealthCheck(connectionString);
        }

        [Fact]
        public async Task RunNetworkHealthCheck()
        {
            var check = new NetworkHealthCheck(connectionString);
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }
    }
}
