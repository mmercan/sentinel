using System;
using System.Threading.Tasks;
using Mercan.HealthChecks.Network.Ping;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Mercan.HealthChecks.Network.Tests.Checks
{
    public class PingHelperShould
    {
        string connectionString = "13.77.151.111:15672";
        HealthCheckContext context = new HealthCheckContext();

        [Fact]
        public void CreateaPingInstance()
        {
            PingHelper check = new PingHelper();
        }

        [Fact]
        public void RunPingHealthCheck()
        {
            var helper = new PingHelper();
            helper.TcpPing(connectionString);

            Assert.Throws<Exception>(() => { helper.TcpPing("blah"); });

        }
    }
}
