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
        readonly string nameSpace;
        readonly string topicName;
        readonly string AccessPolicyName;
        readonly string accessPolicyKey;
        readonly string subscriptionName;
        readonly private ITestOutputHelper output;

        public HealthChecksServiceBusShould(ITestOutputHelper output)
        {
            nameSpace = ConfigurationProvider.Config["servicebus:nameSpace"];
            topicName = ConfigurationProvider.Config["servicebus:topicName"];
            AccessPolicyName = ConfigurationProvider.Config["servicebus:AccessPolicyName"];
            accessPolicyKey = ConfigurationProvider.Config["servicebus:accessPolicyKey"];
            subscriptionName = ConfigurationProvider.Config["servicebus:subscriptionName"];

            this.output = output;
        }

        HealthCheckContext context = new HealthCheckContext();

        private ServiceBusHealthCheck CreateaServiceBusReceiveInstance()
        {
            var check = new ServiceBusHealthCheck(nameSpace, topicName, AccessPolicyName, accessPolicyKey, subscriptionName);
            return check;
        }

        private ServiceBusHealthCheck CreateaServiceBusReceiveInstanceFromconstring()
        {
            var connectionString = $"Endpoint=sb://{nameSpace}.servicebus.windows.net/;SharedAccessKeyName={AccessPolicyName};SharedAccessKey={accessPolicyKey};TransportType=AmqpWebSockets";
            var check = new ServiceBusHealthCheck(connectionString, topicName);
            return check;
        }
        private ServiceBusHealthCheck CreateaServiceBusSendInstance()
        {
            var check = new ServiceBusHealthCheck(nameSpace, topicName, AccessPolicyName, accessPolicyKey);
            return check;
        }


        [Fact]
        public async Task RunServiceBusSendConStringCheck()
        {
            var instance = CreateaServiceBusReceiveInstanceFromconstring();
            var result = await instance.CheckHealthAsync(context);


            // Assert.Equal(2, nameSpace);
            // var check = new RedisHealthCheck(connectionString);
            // var result = await check.CheckHealthAsync(context);
            // Assert.Equal(HealthStatus.Healthy, result.Status);
        }
        [Fact]
        public async Task RunServiceBusSendCheck()
        {
            var instance = CreateaServiceBusSendInstance();
            var result = await instance.CheckHealthAsync(context);


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


            // Assert.Equal(2, nameSpace);
            // var check = new RedisHealthCheck(connectionString);
            // var result = await check.CheckHealthAsync(context);
            // Assert.Equal(HealthStatus.Healthy, result.Status);
        }

        [Fact]
        public async Task AddtothePipelineWorks_conString()
        {
            var connectionString = $"Endpoint=sb://{nameSpace}.servicebus.windows.net/;SharedAccessKeyName={AccessPolicyName};SharedAccessKey={accessPolicyKey};TransportType=AmqpWebSockets";
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks().AddServiceBusHealthCheck(connectionString, topicName);
            var serviceProvider = services1.BuildServiceProvider();

            var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            var result = await healthCheckService.CheckHealthAsync();
            Assert.Equal(HealthStatus.Healthy, result.Status);

        }

        [Fact]
        public async Task AddtothePipelineWorks_send()
        {
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks().AddServiceBusHealthCheck(nameSpace, topicName, AccessPolicyName, accessPolicyKey);
            var serviceProvider = services1.BuildServiceProvider();

            var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            var result = await healthCheckService.CheckHealthAsync();
            Assert.Equal(HealthStatus.Healthy, result.Status);

        }


        [Fact]
        public async Task AddtothePipelineWorks_receive()
        {
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks().AddServiceBusHealthCheck(nameSpace, topicName, subscriptionName, AccessPolicyName, accessPolicyKey);
            var serviceProvider = services1.BuildServiceProvider();

            var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            var result = await healthCheckService.CheckHealthAsync();
            Assert.Equal(HealthStatus.Healthy, result.Status);

        }

    }
}