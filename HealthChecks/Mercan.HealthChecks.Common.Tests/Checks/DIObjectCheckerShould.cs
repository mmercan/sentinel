using System;
using System.Threading.Tasks;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class DIObjectCheckerShould
    {

        private readonly ITestOutputHelper output;
        HealthCheckContext context = new HealthCheckContext();
        IServiceCollection services = new ServiceCollection();
        ILoggerFactory factory = new LoggerFactory();
        public DIObjectCheckerShould(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void CreateaDIObjectCheckerInstanceWithHealthly()
        {
            ILogger<DIObjectChecker<TestClass>> logger = new Logger<DIObjectChecker<TestClass>>(factory);
            // var check = new DIHealthCheck(logger, services);
            var myclass = new TestClass();
            var services1 = new ServiceCollection()
             .AddLogging();
            services1.AddScoped<TestClass>();
            services1.AddScoped<DIObjectChecker<TestClass>>((sp) =>
            {
                return new DIObjectChecker<TestClass>(logger, services1, (tc) =>
                {
                    return tc.runrun() == true;
                });
            });

            services1.AddHealthChecks().AddDIHealthCheck(services1);
            var serviceProvider = services1.BuildServiceProvider();
            var checker = serviceProvider.GetService<DIObjectChecker<TestClass>>();
            var resultTask = checker.CheckHealthAsync(context);
            resultTask.Wait();
            output.WriteLine("Description" + resultTask.Result.Description);
            Assert.True(resultTask.Result.Status == HealthStatus.Healthy);
        }



        [Fact]
        public void CreateaDIObjectCheckerInstanceWithUnhealthly()
        {
            ILogger<DIObjectChecker<TestClass>> logger = new Logger<DIObjectChecker<TestClass>>(factory);
            // var check = new DIHealthCheck(logger, services);
            var myclass = new TestClass();
            var services1 = new ServiceCollection()
             .AddLogging();
            services1.AddScoped<TestClass>();
            services1.AddScoped<DIObjectChecker<TestClass>>((sp) =>
            {
                return new DIObjectChecker<TestClass>(logger, services1, (tc) =>
                {
                    return tc.runrun() == false;
                });
            });

            services1.AddHealthChecks().AddDIHealthCheck(services1);
            var serviceProvider = services1.BuildServiceProvider();
            var checker = serviceProvider.GetService<DIObjectChecker<TestClass>>();
            var resultTask = checker.CheckHealthAsync(context);
            resultTask.Wait();
            output.WriteLine("Description" + resultTask.Result.Description);
            Assert.True(resultTask.Result.Status == HealthStatus.Unhealthy);
        }


        [Fact]
        public void CreateaDIObjectCheckerInstanceWithThrows()
        {
            ILogger<DIObjectChecker<TestClass>> logger = new Logger<DIObjectChecker<TestClass>>(factory);
            // var check = new DIHealthCheck(logger, services);
            var myclass = new TestClass();
            var services1 = new ServiceCollection()
             .AddLogging();
            services1.AddScoped<TestClass>();
            services1.AddScoped<DIObjectChecker<TestClass>>((sp) =>
            {
                return new DIObjectChecker<TestClass>(logger, services1, (tc) =>
                {
                    throw new ApplicationException("Triggered exception");
                });
            });

            services1.AddHealthChecks().AddDIHealthCheck(services1);
            var serviceProvider = services1.BuildServiceProvider();
            var checker = serviceProvider.GetService<DIObjectChecker<TestClass>>();
            var resultTask = checker.CheckHealthAsync(context);
            resultTask.Wait();
            output.WriteLine("Description" + resultTask.Result.Description);
            Assert.True(resultTask.Result.Status == HealthStatus.Unhealthy);
        }


        [Fact]
        public async Task RunDIHealthCheckHealthCheck()
        {

            var services1 = new ServiceCollection()
           .AddLogging();
            services1.AddScoped<TestClass>();

            services1.AddHealthChecks().AddDIObjectHealthCheck<TestClass>((tc) =>
            {
                return tc.runrun() == true;
            });
            var serviceProvider = services1.BuildServiceProvider();


            var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            var result = await healthCheckService.CheckHealthAsync();
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }

    }


    public class TestClass
    {

        public bool runrun()
        {
            return true;
        }
    }

}