using System;
using Xunit;
using Mercan.HealthChecks.Elasticsearch;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;

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
            var check = new ElasticsearchHealthCheck(connectionString);
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }
    }
}
