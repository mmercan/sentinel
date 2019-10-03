using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Sentinel.Api.Member;
using Microsoft.Extensions.Configuration;

namespace Sentinel.Api.Member.Tests.Helpers
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                //.UseStartup<Startup>()
                .ConfigureAppConfiguration(config => config
                    .AddJsonFile("appsettings.docker.tests.json", false)
                );
        }
    }
}