using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Sentinel.Api.Product;
using Microsoft.Extensions.Configuration;

namespace Sentinel.Api.Product.Tests.Helpers
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