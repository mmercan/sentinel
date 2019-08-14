using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Sentinel.Api.HealthMonitoring;
using Microsoft.Extensions.Configuration;

namespace Sentinel.Handler.Comms.Tests.Helpers
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Sentinel.Handler.Comms.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseStartup<Startup>()
            // .ConfigureLogging(c=>c.Services.Add())

            // .ConfigureAppConfiguration(config =>config)
            .UseEnvironment("DockerTest");

            //  .ConfigureAppConfiguration(config => config
            //      .AddJsonFile("appsettings.DockerTest.json")
            //  );
        }
    }
}