using Mercan.HealthChecks.Common.Checks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;



namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class NumericChecksShould
    {
        public NumericChecksShould()
        {

        }


        [Fact]
        public void CreateANewInstance()
        {
            var services = new ServiceCollection()
            .AddLogging();

            services.AddHealthChecks()
            .AddMinValueCheck("testitMin", 500, () => { return 300; })
            .AddMaxValueCheck("testitMax", 500, () => { return 900; });


            var serviceProvider = services.BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<DIHealthCheck>();


            var healthChecksBuilder = serviceProvider.GetService<IHealthChecksBuilder>();
            var healthCheckService = serviceProvider.GetService<HealthCheckService>();



            var resultTask = healthCheckService.CheckHealthAsync();

            // resultTask.Wait();
            // var q = resultTask.Result;

            // HealthCheckService
            //HealthCheckBuilderExtensions.AddMinValueCheck(healthChecksBuilder,"testitMin", 500, ()=>{return 300;});
            // DIHealthCheck healthcheck = new DIHealthCheck(logger, services);
            // var context = new HealthCheckContext();
            // var task = healthcheck.CheckHealthAsync(context);
            // task.Wait();
            // var q = task.Result;
        }
    }
}