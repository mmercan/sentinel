// using System;
// using System.Threading.Tasks;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Diagnostics.HealthChecks;
// using Xunit;

// namespace Mercan.HealthChecks.Nats.Tests.Checks
// {
//     public class NatsShould
//     {
//         string connectionString = "13.77.147.26:4222/";
//         HealthCheckContext context = new HealthCheckContext();
//         [Fact]
//         public void CreateaNatsInstance()
//         {
//             var check = new NatsHealthCheck(connectionString);
//         }

//         [Fact]
//         public async Task RunNatsHealthCheck()
//         {
//             var check = new NatsHealthCheck(connectionString);
//             var result = await check.CheckHealthAsync(context);
//             Assert.Equal(HealthStatus.Healthy, result.Status);
//         }


//         [Fact]
//         public void AddtothePipelineWorks()
//         {
//             var services1 = new ServiceCollection()
//             .AddLogging();
//             services1.AddHealthChecks().AddNatsHealthCheck(connectionString);
//             var serviceProvider = services1.BuildServiceProvider();
//             //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
//             // var result = await healthCheckService.CheckHealthAsync();
//             // Assert.Equal(HealthStatus.Healthy, result.Status);

//         }
//     }
// }
