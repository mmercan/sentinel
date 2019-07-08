using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sentinel.Api.HealthMonitoring.Tests.Helpers
{
    [CollectionDefinition("WebApplicationFactory")]
    public class WebApplicationFactoryCollection : ICollectionFixture<WebApplicationFactory<Sentinel.Api.HealthMonitoring.Startup>>, ICollectionFixture<AuthTokenFixture>, ICollectionFixture<CustomWebApplicationFactory>
    {


    }
}