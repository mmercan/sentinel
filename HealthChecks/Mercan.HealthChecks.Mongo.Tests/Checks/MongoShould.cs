using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Mercan.HealthChecks.Mongo.Tests.Checks
{
    public class MongoShould
    {

        string connectionString = "mongodb://root:hbMnztmZ4w9JJTGZ@52.183.8.101:27017/admin?readPreference=primary";
        string failedConnectionString = "mongodb://root:hbMnztmZ4w9JJTGZ@mongooo/admin?readPreference=primary";
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


        [Fact]
        public async Task RunMongoHealthCheckWithWrongConString()
        {
            var check = new MongoHealthCheck(failedConnectionString);
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Unhealthy, result.Status);
        }

        [Fact]
        public void AddtothePipelineWorks()
        {
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks().AddMongoHealthCheck(connectionString);
            var serviceProvider = services1.BuildServiceProvider();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }
    }
}
