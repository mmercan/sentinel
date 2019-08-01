using System.Threading.Tasks;
using Mercan.HealthChecks.Common.Checks;
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
    }
}