using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Sentinel.Api.HealthMonitoring;
using Microsoft.Extensions.Configuration;

namespace Sentinel.Api.HealthMonitoring.Tests.Helpers
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //  builder
            //      .UseStartup<Startup>();

            // .UseEnvironment("dockertest");
            // .ConfigureLogging(c=>c.Services.Add())

            // .ConfigureAppConfiguration(config =>config)


            //  .ConfigureAppConfiguration(config => config
            //      .AddJsonFile("appsettings.DockerTest.json")
            //  );
        }
    }
}