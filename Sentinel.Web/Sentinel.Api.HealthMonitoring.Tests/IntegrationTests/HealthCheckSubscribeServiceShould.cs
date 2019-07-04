using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Sentinel.Api.HealthMonitoring;
using Sentinel.Api.HealthMonitoring.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EasyNetQ;
using Moq;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Sentinel.Api.HealthMonitoring.Tests.IntegrationTests
{
    [Collection("WebApplicationFactory")]
    public class HealthCheckSubscribeServiceShould
    {
        private WebApplicationFactory<Startup> factory;
        private ITestOutputHelper output;

        public HealthCheckSubscribeServiceShould(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
        }


        private HealthCheckSubscribeService instance()
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                //.SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("appsettings.json")
                .Build();

            var serviceProvider = new ServiceCollection()
                 .AddLogging()
                 .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<HealthCheckSubscribeService>();

            var moqIbus = new Mock<IBus>();

            var ibus = moqIbus.Object;
            HealthCheckSubscribeService profile = new HealthCheckSubscribeService(logger, configuration, ibus);
            return profile;
        }

        [Fact]
        public void CreateAnInstance()
        {
            var item = instance();
            Assert.NotNull(item);
        }

        [Fact]
        public void CanCallStart()
        {
            var item = instance();
            var task = item.StartAsync(new System.Threading.CancellationToken());
            task.Wait();
            //Assert.NotNull(item);
        }

        [Fact]
        public void CanCallStop()
        {
            var item = instance();
            var task = item.StopAsync(new System.Threading.CancellationToken());
            task.Wait();
            // Assert.NotNull(item);
        }

        [Fact]
        public void CanCallDispose()
        {
            var item = instance();

            item.Dispose();
            // Assert.NotNull(item);
        }
    }
}