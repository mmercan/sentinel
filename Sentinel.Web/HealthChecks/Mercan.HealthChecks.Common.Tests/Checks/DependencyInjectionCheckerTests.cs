using Mercan.HealthChecks.Common.Checks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class DependencyInjectionCheckerTests
    {
        IServiceCollection services = new ServiceCollection();

        ILoggerFactory factory = new LoggerFactory();
        // factory.CreateLogger("blah");

        //ILogger<DIHealthCheck> logger = new Logger<DIHealthCheck>(factory);
        //services.AddLogging
        // DIHealthCheck healthcheck = new DIHealthCheck(services);
    }
}