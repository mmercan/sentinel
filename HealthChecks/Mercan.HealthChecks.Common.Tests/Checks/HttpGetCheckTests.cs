using System;
using System.Threading.Tasks;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class HttpGetCheckTests
    {
        private readonly ITestOutputHelper output;
        ILoggerFactory factory = new LoggerFactory();
        string conection = "https://www.google.com";  //Environment.GetEnvironmentVariable("SentinelConnection");
        public HttpGetCheckTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public async Task getHttpGoogle()
        {
            HealthCheckContext context = new HealthCheckContext();
            ILogger<HttpGetCheck> logger = new Logger<HttpGetCheck>(factory);
            HttpGetCheck httpgetcheck = new HttpGetCheck(logger, conection);
            var result = await httpgetcheck.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }

        [Fact]
        public async Task AddMiddlewareAsync()
        {
            var services = new ServiceCollection()
            .AddLogging();

            services.AddHealthChecks()
            .AddHttpGetCheck(conection);


            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<DIHealthCheck>();
            var healthChecksBuilder = serviceProvider.GetService<IHealthChecksBuilder>();
            var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            var resultTask = healthCheckService.CheckHealthAsync();

            await Task.Run(() =>
            {

            });

        }


    }
}