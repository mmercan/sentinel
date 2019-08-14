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
    public class DependencyInjectionCheckerShould
    {

        HealthCheckContext context = new HealthCheckContext();
        IServiceCollection services = new ServiceCollection();
        ILoggerFactory factory = new LoggerFactory();
        // // 
        [Fact]
        public void CreateaDIHealthCheckInstance()
        {
            ILogger<DIHealthCheck> logger = new Logger<DIHealthCheck>(factory);
            var check = new DIHealthCheck(logger, services);
        }

        [Fact]
        public async Task RunDIHealthCheckHealthCheck()
        {
            services.AddSingleton<DependencyInjectionCheckerShould>();
            ILogger<DIHealthCheck> logger = new Logger<DIHealthCheck>(factory);
            var check = new DIHealthCheck(logger, services);
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }


        [Fact]
        public void AddtothePipelineWorks()
        {
            var services1 = new ServiceCollection()
            .AddLogging();
            services1.AddHealthChecks().AddDIHealthCheck(services1);
            var serviceProvider = services1.BuildServiceProvider();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }

        // IServiceCollection services = new ServiceCollection();

        // ILoggerFactory factory = new LoggerFactory();
        // // factory.CreateLogger("blah");

        // ILogger<DIHealthCheck> logger = new Logger<DIHealthCheck>(factory);
        // //services.AddLogging
        // // DIHealthCheck healthcheck = new DIHealthCheck(services);


        //  var _fixture = new Fixture();

        //var _mockLogger = new Mock<ILogger<DIHealthCheck>>();
        //     _mockCatalogueService = new Mock<ICatalogueService>();
        //     _catalogueController = new CatalogueController(_mockLogger.Object, _mockCatalogueService.Object);

        // [Fact]
        // public async Task CreateANewInstanceAsync()
        // {

        //     IServiceCollection services = new ServiceCollection();
        //     var serviceProvider = new ServiceCollection()
        //     .AddLogging()
        //     .BuildServiceProvider();

        //     var factory = serviceProvider.GetService<ILoggerFactory>();
        //     var logger = factory.CreateLogger<DIHealthCheck>();

        //     DIHealthCheck healthcheck = new DIHealthCheck(logger, services);
        //     var context = new HealthCheckContext();
        //     var result = await  healthcheck.CheckHealthAsync(context);
        // }

    }
}