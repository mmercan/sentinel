using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Mercan.HealthChecks.Common.Checks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Mercan.HealthChecks.Common.Tests.Checks
{
    public class ServiceClientCheckerShould
    {

        HealthCheckContext context = new HealthCheckContext();
        IServiceCollection services = new ServiceCollection();
        ILoggerFactory factory = new LoggerFactory();
        ApiServiceConfiguration config = new ApiServiceConfiguration();

        public ServiceClientCheckerShould()
        {
            config.BaseAddress = "https://google.com";
            config.ClientCertificateBase64 = "MIICYzCCAcygAwIBAgIBADANBgkqhkiG9w0BAQUFADAuMQswCQYDVQQGEwJVUzEM  MAoGA1UEChMDSUJNMREwDwYDVQQLEwhMb2NhbCBDQTAeFw05OTEyMjIwNTAwMDBa  Fw0wMDEyMjMwNDU5NTlaMC4xCzAJBgNVBAYTAlVTMQwwCgYDVQQKEwNJQk0xETAP  BgNVBAsTCExvY2FsIENBMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQD2bZEo  7xGaX2/0GHkrNFZvlxBou9v1Jmt/PDiTMPve8r9FeJAQ0QdvFST/0JPQYD20rH0b  imdDLgNdNynmyRoS2S/IInfpmf69iyc2G0TPyRvmHIiOZbdCd+YBHQi1adkj17ND  cWj6S14tVurFX73zx0sNoMS79q3tuXKrDsxeuwIDAQABo4GQMIGNMEsGCVUdDwGG  +EIBDQQ+EzxHZW5lcmF0ZWQgYnkgdGhlIFNlY3VyZVdheSBTZWN1cml0eSBTZXJ2  ZXIgZm9yIE9TLzM5MCAoUkFDRikwDgYDVR0PAQH/BAQDAgAGMA8GA1UdEwEB/wQF  MAMBAf8wHQYDVR0OBBYEFJ3+ocRyCTJw067dLSwr/nalx6YMMA0GCSqGSIb3DQEB  BQUAA4GBAMaQzt+zaj1GU77yzlr8iiMBXgdQrwsZZWJo5exnAucJAEYQZmOfyLiM  D6oYq+ZnfvM0n8G/Y79q8nhwvuxpYOnRSAXFp6xSkrIOeZtJMY1h00LKp/JX3Ng1  svZ2agE126JHsQ0bhzN5TKsYfbwfTwfjdWAGy6Vf1nYi/rO+ryMO";
        }


        [Fact]
        public void CreateanInstance()
        {
            ILogger<ServiceClientBaseHealthCheck> logger = new Logger<ServiceClientBaseHealthCheck>(factory);
            var check = new ServiceClientBaseHealthCheck(logger, config, "/");
        }

        [Fact]
        public async Task RunHealthCheck()
        {
            ILogger<ServiceClientBaseHealthCheck> logger = new Logger<ServiceClientBaseHealthCheck>(factory);
            var check = new ServiceClientBaseHealthCheck(logger, config, "/");
            var result = await check.CheckHealthAsync(context);
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }

        [Fact]
        public void AddtothePipelineWorks()
        {

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build();

            configuration["sentinel-api-member__ClientOptions__BaseAddress"] = "www.google.com";

            var services1 = new ServiceCollection().AddLogging();
            services1.AddHealthChecks().AddApiIsAlive(configuration.GetSection("sentinel-api-member"), "/");
            var serviceProvider = services1.BuildServiceProvider();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }

        [Fact]
        public void AddtothePipelineforIsaliveAndWellWorks()
        {

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build();

            configuration["sentinel-api-member__ClientOptions__BaseAddress"] = "www.google.com";

            var services1 = new ServiceCollection().AddLogging();
            services1.AddHealthChecks().AddApiIsAliveAndWell(configuration.GetSection("sentinel-api-member"), "/");
            var serviceProvider = services1.BuildServiceProvider();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }


        [Fact]
        public void AddtothePipelineNotWorksWithinvalidUrl()
        {

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build();

            configuration["sentinel-api-member__ClientOptions__BaseAddress"] = "www.google.com/blah";

            var services1 = new ServiceCollection().AddLogging();
            services1.AddHealthChecks().AddApiIsAlive(configuration.GetSection("sentinel-api-member"), "/");
            var serviceProvider = services1.BuildServiceProvider();
            //  var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            // var result = await healthCheckService.CheckHealthAsync();
            // Assert.Equal(HealthStatus.Healthy, result.Status);

        }
        public void ServiceClientBaseHealthCheckWorks()
        {
            ILogger<ServiceClientBaseHealthCheck> logger = new Logger<ServiceClientBaseHealthCheck>(factory);
            var check = new ServiceClientBaseHealthCheck(logger, config, "/");
            // check.GetToken();
            // object o = null;
            // check.CreateContent<object>(o);
            //check.AddJsonProtocol()
            check.SendAsync<MockData>("https://jsonplaceholder.typicode.com/todos/1", HttpMethod.Get, new MockData())


        }
    }

    public class MockData
    {
        public string userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }

    }
}