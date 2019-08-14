using System;
using Xunit;
using Mercan.HealthChecks.Elasticsearch;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mercan.HealthChecks.Elasticsearch.Tests.Checks
{
    public class ElasticsearchShould
    {
        string connectionString = "http://52.183.4.219:9200";
        HealthCheckContext context = new HealthCheckContext();
        [Fact]
        public void CreateaElasticsearchInstance()
        {
            var check = new ElasticsearchHealthCheck(connectionString);
        }

        [Fact]
        public async Task RunElasticsearchHealthCheck()
        {
            var check = new ElasticsearchHealthCheck("http://elasti:9200");
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }

        [Fact]
        public async Task RunElasticsearchHealthCheckWithWrongConString()
        {
            var check = new ElasticsearchHealthCheck(connectionString);
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }

        [Fact]
        public void AddtothePipelineWorks()
        {
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks().AddElasticsearchHealthCheck(connectionString);
            var serviceProvider = services1.BuildServiceProvider();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }
    }
}
