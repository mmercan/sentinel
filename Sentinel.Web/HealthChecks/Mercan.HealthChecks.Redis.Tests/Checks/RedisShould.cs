using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Mercan.HealthChecks.Redis.Tests.Checks
{
    public class RedisShould
    {

        string connectionString = "52.247.233.71:6379,defaultDatabase=2,password=yourpassword";
        HealthCheckContext context = new HealthCheckContext();
        [Fact]
        public void CreateaRedisInstance()
        {
            RedisHealthCheck check = new RedisHealthCheck(connectionString);
        }

        [Fact]
        public async Task RunRedisHealthCheck()
        {
            var check = new RedisHealthCheck(connectionString);
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }
    }
}
