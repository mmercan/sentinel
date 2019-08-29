using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;
using Xunit.Abstractions;

namespace Mercan.HealthChecks.ServiceBus.Tests
{
    public class HealthChecksServiceBusShould
    {
        string nameSpace;
        string topicName;
        string AccessPolicyName;
        string accessPolicyKey;
        string subscriptionName;
        private ITestOutputHelper output;

        public HealthChecksServiceBusShould(ITestOutputHelper output)
        {
            LoadConfig();
            this.output = output;
        }

        private void LoadConfig()
        {
            nameSpace = ConfigurationProvider.Config["servicebus:nameSpace"];
            topicName = ConfigurationProvider.Config["servicebus:topicName"];
            AccessPolicyName = ConfigurationProvider.Config["servicebus:AccessPolicyName"];
            accessPolicyKey = ConfigurationProvider.Config["servicebus:accessPolicyKey"];
            subscriptionName = ConfigurationProvider.Config["servicebus:subscriptionName"];
        }
        HealthCheckContext context = new HealthCheckContext();

        private ServiceBusHealthCheck CreateaServiceBusReceiveInstance()
        {
            var check = new ServiceBusHealthCheck(nameSpace, topicName, AccessPolicyName, accessPolicyKey, subscriptionName);
            return check;
        }

        private ServiceBusHealthCheck CreateaServiceBusSendInstance()
        {
            var check = new ServiceBusHealthCheck(nameSpace, topicName, AccessPolicyName, accessPolicyKey);
            return check;
        }

        [Fact]
        public async Task RunServiceBusSendCheck()
        {
            var instance = CreateaServiceBusSendInstance();
            var result = await instance.CheckHealthAsync(context);

            output.WriteLine(nameSpace);
            // Assert.Equal(2, nameSpace);
            // var check = new RedisHealthCheck(connectionString);
            // var result = await check.CheckHealthAsync(context);
            // Assert.Equal(HealthStatus.Healthy, result.Status);
        }

        [Fact]
        public async Task RunServiceBusReceiveCheck()
        {
            var instance = CreateaServiceBusReceiveInstance();
            var result = await instance.CheckHealthAsync(context);

            output.WriteLine(nameSpace);
            // Assert.Equal(2, nameSpace);
            // var check = new RedisHealthCheck(connectionString);
            // var result = await check.CheckHealthAsync(context);
            // Assert.Equal(HealthStatus.Healthy, result.Status);
        }

        [Fact]
        public void AddtothePipelineWorks()
        {
            // var services1 = new ServiceCollection()
            // .AddLogging();
            // services1.AddHealthChecks().AddRedisHealthCheck(connectionString);
            // var serviceProvider = services1.BuildServiceProvider();

            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }

    }
}