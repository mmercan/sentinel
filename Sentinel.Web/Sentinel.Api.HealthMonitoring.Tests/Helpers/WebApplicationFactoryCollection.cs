using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Sentinel.Api.HealthMonitoring.Tests.Helpers
{
    [CollectionDefinition("WebApplicationFactory")]
    public class WebApplicationFactoryCollection : ICollectionFixture<AuthTokenFixture>, ICollectionFixture<CustomWebApplicationFactory>
    {


    }
}