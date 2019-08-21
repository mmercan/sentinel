using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
        public async Task AddtothePipelineWorks1()
        {
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks()
             .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory")
             .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "AvailableMBytes")
             .AddPerformanceCounter("Win32_PerfRawData_PerfOS_Memory", "PercentCommittedBytesInUse", "PercentCommittedBytesInUse_Base");


            var serviceProvider = services1.BuildServiceProvider();
            var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }


        [Fact]
        public async Task SystemInfoHealthChecksWorks()
        {
            var services = new ServiceCollection()
            .AddLogging();

            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<SystemInfoHealthChecks>();

            HealthCheckContext context = new HealthCheckContext();
            SystemInfoHealthChecks infohc = new SystemInfoHealthChecks(logger);
            var result = await infohc.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }
    }
}