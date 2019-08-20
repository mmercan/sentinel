using System;
using System.Threading.Tasks;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class SqlConnectionHealthCheckTests
    {


        private readonly ITestOutputHelper output;
        string conection = "Server=52.175.193.162;Database=sentinel;User Id=sa;Password=MySentP@ssw0rd;";  //Environment.GetEnvironmentVariable("SentinelConnection");
        public SqlConnectionHealthCheckTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public async Task ConnectDatabase()
        {
            HealthCheckContext context = new HealthCheckContext();

            SqlConnectionHealthCheck sql = new SqlConnectionHealthCheck(conection);
            var result = await sql.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);

            // task.Wait();
            // // 
            // output.WriteLine("Description : " + task.Result.Description);

            // foreach (var item in task.Result.Data)
            // {
            //     output.WriteLine("Data  " + item.Key + ":" + item.Value.ToString());
            // }
            // output.WriteLine("Data Counts : " + task.Result.Data.Count);
            // Assert.Equal(HealthStatus.Healthy, task.Result.Status);

        }

        [Fact]
        public async Task AddMiddlewareAsync()
        {
            var services = new ServiceCollection()
            .AddLogging();

            services.AddHealthChecks()
            .SqlConnectionHealthCheck(conection)
            .SqlConnectionHealthCheck(conection, "select 1");


            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<DIHealthCheck>();
            var healthChecksBuilder = serviceProvider.GetService<IHealthChecksBuilder>();
            var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            var resultTask = healthCheckService.CheckHealthAsync();

            await Task.Run(() =>
            {

            });

        }


    }
}