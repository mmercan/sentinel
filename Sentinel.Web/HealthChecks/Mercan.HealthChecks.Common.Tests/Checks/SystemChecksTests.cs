using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Mercan.HealthChecks.Common.Checks;

namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class SystemChecksTests
    {


        [Fact]
        public void AddtothePipelineWorks()
        {
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks()
            .AddPrivateMemorySizeCheck(1000000)
            .AddPrivateMemorySizeCheckKB(1000000)
            .AddPrivateMemorySizeCheckMB(1000)
            .AddVirtualMemorySizeCheck(1000000)
            .AddVirtualMemorySizeCheckKB(1000000)
            .AddVirtualMemorySizeCheckMB(1000);

            var serviceProvider = services1.BuildServiceProvider();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }
    }
}