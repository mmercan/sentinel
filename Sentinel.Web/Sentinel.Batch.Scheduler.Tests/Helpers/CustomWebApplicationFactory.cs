using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Sentinel.Batch.Scheduler;
using Microsoft.Extensions.Configuration;

namespace Sentinel.Batch.Scheduler.Tests.Helpers
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
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