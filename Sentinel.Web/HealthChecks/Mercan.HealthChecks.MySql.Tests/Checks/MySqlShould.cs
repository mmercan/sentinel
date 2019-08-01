using System;
using System.Threading.Tasks;
using Mercan.HealthChecks.Mysql;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Mercan.HealthChecks.MySql.Tests.Checks
{
    public class MySqlShould
    {
        string connectionString = "mongodb://root:hbMnztmZ4w9JJTGZ@52.247.221.171:27017/admin?readPreference=primary";
        HealthCheckContext context = new HealthCheckContext();

        [Fact]
        public void CreateaMySqlInstance()
        {
            var check = new MysqlHealthCheck(connectionString);
        }

        [Fact]
        public async Task RunMySqlHealthCheck()
        {
            var check = new MysqlHealthCheck(connectionString);
            var result = await check.CheckHealthAsync(context);
            // Assert.Equal(HealthStatus.Healthy, result.Status);
        }
    }
}
