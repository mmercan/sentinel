using System.Threading.Tasks;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class ProcessListCheckerShould
    {
        HealthCheckContext context = new HealthCheckContext();

        [Fact]
        public void CreateanInstance()
        {
            var check = new ProcessListHealthChecks();
        }

        [Fact]
        public async Task RunHealthCheck()
        {
            var check = new ProcessListHealthChecks();
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }


        [Fact]
        public void AddtothePipelineWorks()
        {
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks().AddProcessList();
            var serviceProvider = services1.BuildServiceProvider();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }
    }
}