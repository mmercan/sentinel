using System;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;
using Xunit.Abstractions;

namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class SqlConnectionHealthCheckTests
    {


        private readonly ITestOutputHelper output;

        public SqlConnectionHealthCheckTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void ConnectDatabase()
        {

            HealthCheckContext context = new HealthCheckContext();


            var conection = "Server=52.247.221.7;Database=sentinel;User Id=sa;Password=MySentP@ssw0rd;";  //Environment.GetEnvironmentVariable("SentinelConnection");
            SqlConnectionHealthCheck sql = new SqlConnectionHealthCheck(conection);
            var task = sql.CheckHealthAsync(context);
            task.Wait();
            // 
            output.WriteLine("Description : " + task.Result.Description);

            foreach (var item in task.Result.Data)
            {
                output.WriteLine("Data  " + item.Key + ":" + item.Value.ToString());

            }
            output.WriteLine("Data Counts : " + task.Result.Data.Count);
            Assert.Equal(HealthStatus.Healthy, task.Result.Status);

        }


    }
}