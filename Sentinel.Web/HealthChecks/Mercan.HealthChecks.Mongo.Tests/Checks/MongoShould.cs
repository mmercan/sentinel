using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Mercan.HealthChecks.Mongo.Tests.Checks
{
    public class MongoShould
    {

        string connectionString = "mongodb://root:hbMnztmZ4w9JJTGZ@52.247.221.171:27017/admin?readPreference=primary";
        HealthCheckContext context = new HealthCheckContext();

        [Fact]
        public void CreateaMongoInstance()
        {
            var check = new MongoHealthCheck(connectionString);
        }

        [Fact]
        public async Task RunMongoHealthCheck()
        {
            var check = new MongoHealthCheck(connectionString);
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }
    }
}
