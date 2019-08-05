using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;

namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class SystemInfoTests
    {
        [Fact]
        public void AddtothePipelineWorks()
        {
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks()
            .AddSystemInfoCheck();

            var serviceProvider = services1.BuildServiceProvider();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }



        [Fact]
        public async Task SystemInfoHealthChecksWorks()
        {
            HealthCheckContext context = new HealthCheckContext();
            SystemInfoHealthChecks infohc = new SystemInfoHealthChecks();
            var result = await infohc.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }
    }
}