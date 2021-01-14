using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Mercan.HealthChecks.Redis.Tests.Checks
{
    public class RedisShould
    {

        string connectionString = "52.247.72.202:6379,defaultDatabase=2,password=2jWa8sSM8ZuhS3Qc";
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


        [Fact]
        public void AddtothePipelineWorks()
        {
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks().AddRedisHealthCheck(connectionString);
            var serviceProvider = services1.BuildServiceProvider();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }

    }
}
